using Riptide;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Command
{
  public class JoinedToLobbyCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      JoinedToLobbyVo vo = (JoinedToLobbyVo)evt.data;
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.JoinedToLobby);
      message=networkManager.SetData(message,vo.lobby);


      Debug.Log(vo.clientVo.id);
      networkManager.Server.Send(message,vo.clientVo.id);
      Debug.Log("Joined to Lobby Message sent");
      
      Message messageToOthers = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.NewPlayerToLobby);
      messageToOthers=networkManager.SetData(messageToOthers,vo.clientVo);


      
      for (ushort i = 0; i < vo.lobby.playerCount; i++)
      {
        if (i!=vo.clientVo.inLobbyId)
        {
          networkManager.Server.Send(messageToOthers,vo.lobby.clients[i].id);
        }
      }
    }
  }
}