using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using Riptide;
using Riptide.Utils;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Vo;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Runtime.Contexts.Network.Services.NetworkManager
{
  public class NetworkManagerService : INetworkManagerService
  {
    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }

    private int maxPacketSize = 1200;

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

    }

    public void SendToLobby(Message message, Dictionary<ushort,ClientVo> clients)
    {
      for (ushort i = 0; i < clients.Count; i++)
      {
        Server.Send(message,clients[i].id);
      }
    }
    public void SendToLobbyExcept(Message message,ushort exceptClient, Dictionary<ushort,ClientVo> clients)
    {
      for (ushort i = 0; i < clients.Count; i++)
      {
        if (i!=exceptClient)
          Server.Send(message,clients[i].id);
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
      crossDispatcher.Dispatch((ClientToServerId)messageArgs.MessageId, vo);
    }

    public T GetData<T>(byte[] message) where T : new()
    {
      using MemoryStream stream = new MemoryStream(message);
      return message == null ? default : Serializer.Deserialize<T>(stream);
    }

    public Message SetData(Message message, object obj)
    {
      if (obj == null)
        Debug.LogError("Set data object is null");
      byte[] objBytes = ProtoSerialize<object>(obj);

      message.AddBytes(objBytes);
      return message;
    }

    private byte[] ProtoSerialize<T>(T message) where T : new()
    {
      using var stream = new MemoryStream();
      Serializer.Serialize(stream, message);
      return stream.ToArray();
    }


    public void OnQuit()
    {
      Server.Stop();
    }
  }
}