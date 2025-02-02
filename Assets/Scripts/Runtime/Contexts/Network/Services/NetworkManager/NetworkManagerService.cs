using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editor.Tools.DebugX.Runtime;
using ProtoBuf;
using Riptide;
using Riptide.Utils;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace Runtime.Contexts.Network.Services.NetworkManager
{
  public class NetworkManagerService : INetworkManagerService
  {
    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }

    private protected int maxPacketSize = 1200;

    public Server Server { get; private set; }

    private ushort port;
    private ushort maxClientCount;

    public void StartServer(ushort _port, ushort _maxClientCount)
    {
      port = _port;
      maxClientCount = _maxClientCount;

      RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
      Server = new Server();
      Server.Start(port, maxClientCount);

      Server.MessageReceived += MessageHandler;
      Server.ClientDisconnected += OnClientDisconnected;
    }

    private void OnClientDisconnected(object sender, ServerDisconnectedEventArgs eventArgs)
    {
      crossDispatcher.Dispatch(NetworkEvent.ClientDisconnected, eventArgs.Client.Id);
    }

    public void SendToLobby(Message message, Dictionary<ushort, ClientVo> clients)
    {
      for (ushort i = 0; i < clients.Count; i++)
      {
        Server.Send(message, clients.ElementAt(i).Value.id);
      }
    }

    public void SendToLobbyExcept(Message message, ushort exceptClient, Dictionary<ushort, ClientVo> clients)
    {
      for (ushort i = 0; i < clients.Count; i++)
      {
        if (clients.ElementAt(i).Value.id != exceptClient)
          Server.Send(message, clients.ElementAt(i).Value.id);
      }
    }

    public void Ticker()
    {
      Server?.Update();
    }

    public void MessageHandler(object sender, MessageReceivedEventArgs messageArgs)
    {
      MessageReceivedVo vo = new()
      {
        fromId = messageArgs.FromConnection.Id,
        message = messageArgs.Message.GetBytes()
      };
      DebugX.Log(DebugKey.Processor,((ClientToServerId)messageArgs.MessageId)+" MessageReceived");
      crossDispatcher.Dispatch((ClientToServerId)messageArgs.MessageId, vo);
    }

    public T GetData<T>(byte[] message)
    {
      using MemoryStream stream = new(message);
      return message == null ? default : Serializer.Deserialize<T>(stream);
    }

    public Message SetData(Message message, object obj)
    {
      if (obj == null)
        Debug.LogError("Set data object is null");
      byte[] objBytes = ProtoSerialize(obj);

      message.AddBytes(objBytes);
      return message;
    }

    private byte[] ProtoSerialize<T>(T message) where T : new()
    {
      using MemoryStream stream = new();
      Serializer.Serialize(stream, message);
      return stream.ToArray();
    }

    public void OnQuit()
    {
      Server.Stop();
    }
    
    public SendPacketToLobbyVo<T> SetSendPacketToLobbyVo<T>(T myObject, Dictionary<ushort,ClientVo> clients) where T : new()
    {
      SendPacketToLobbyVo<T> sendPacketToLobbyVo = new()
      {
        mainClass = myObject,
        clients = clients
      };
      
      return sendPacketToLobbyVo;
    }
  }
}