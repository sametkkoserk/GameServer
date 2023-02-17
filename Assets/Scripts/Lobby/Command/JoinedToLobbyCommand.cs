using Lobby.Vo;
using Network.Enum;
using Network.Services.NetworkManager;
using Network.Vo;
using Riptide;
using strange.extensions.command.impl;
using UnityEngine;

namespace Lobby.Command
{
  public class JoinedToLobbyCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      JoinedToLobbyVo vo = (JoinedToLobbyVo)evt.data;
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.JoinedToLobby);

      LobbyVo lobbyVo = vo.lobby;
      message.AddUShort(lobbyVo.lobbyId);
      message.AddString(lobbyVo.lobbyName);
      message.AddBool(lobbyVo.isPrivate);
      message.AddUShort(lobbyVo.leaderId);
      message.AddUShort(lobbyVo.playerCount);
      message.AddUShort(lobbyVo.maxPlayerCount);
      message.AddUShort(vo.clientVo.inLobbyId);
      for (ushort i = 0; i < lobbyVo.playerCount; i++)
      {
        ClientVo clientVo = lobbyVo.clients[i];
        message.AddUShort(clientVo.id);
        message.AddUShort(clientVo.inLobbyId);
        //message.AddString(clientVo.userName);
        message.AddUShort(clientVo.colorId);
      }

      networkManager.Server.Send(message,vo.clientVo.id);
      Debug.Log("Joined to Lobby Message sent");
      
      Message messageToOthers = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.NewPlayerToLobby);

      messageToOthers.AddUShort(vo.clientVo.id);
      messageToOthers.AddUShort(vo.clientVo.inLobbyId);
      //messageToOthers.AddString(vo.clientVo.userName);
      messageToOthers.AddUShort(vo.clientVo.colorId);
      
      for (ushort i = 0; i < lobbyVo.playerCount; i++)
      {
        if (i!=vo.clientVo.inLobbyId)
        {
          networkManager.Server.Send(messageToOthers,lobbyVo.clients[i].id);
        }
      }
    }
  }
}