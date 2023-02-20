using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lobby.Vo;
using Network.Enum;
using Network.Vo;
using Riptide;
using Riptide.Utils;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Unity.VisualScripting;
using UnityEngine;

namespace Network.Services.NetworkManager
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
        message = messageArgs.Message
      };
      crossDispatcher.Dispatch((ClientToServerId)messageArgs.MessageId, vo);
    }

    public void OnQuit()
    {
      Server.Stop();
    }
  }
}