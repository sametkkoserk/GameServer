using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Riptide;
using Riptide.Utils;
using Runtime.Network.Enum;
using Runtime.Network.Vo;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Runtime.Network.Services.NetworkManager
{
  public class NetworkManagerService : INetworkManagerService
  {
    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }

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


      // TODO: Şafak: Silinecek. Bunlar deneme çalışmaları.
      // LobbyVo y = new()
      // {
      //     lobbyId = 3
      // };
      //
      // sl<LobbyVo>(y);
    }

    public void sl<T>(T fre)
    {
      // TODO: Şafak: Silinecek. Bunlar deneme çalışmaları.
      Type type = typeof(T);
      MemberInfo[] privateMembers = type.GetMembers();
      var z = privateMembers.ToList();

      Debug.Log(z.Count);

      for (int i = 0; i < z.Count; i++)
      {
        Debug.Log(z[i]);
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
        message = messageArgs.Message.GetString()
      };
      crossDispatcher.Dispatch((ClientToServerId)messageArgs.MessageId, vo);
    }
    
    public T GetData<T>(string message) where T : new()
    {
      if ( message== null)
        return default(T);

      return JsonConvert.DeserializeObject<T>(message);
    }
    public Message SetData(Message message,object obj)
    {
      if ( obj== null)
        Debug.LogError("Set data object is null");
      string objStr=JsonConvert.SerializeObject(obj);
      message.AddString(objStr);

      return message;
    }


    public void OnQuit()
    {
      Server.Stop();
    }
  }
}