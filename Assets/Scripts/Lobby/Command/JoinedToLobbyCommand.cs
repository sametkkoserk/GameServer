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
      message.AddUShort(vo.lobby.lobbyId);
      message.AddString(vo.lobby.lobbyName);
      message.AddBool(vo.lobby.isPrivate);
      message.AddUShort(vo.lobby.leaderId);
      networkManager.Server.Send(message,vo.clientId);
      Debug.Log("Joined to Lobby Message sent");
    }
  }
}