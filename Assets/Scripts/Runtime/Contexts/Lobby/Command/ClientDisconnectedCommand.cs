using System.Collections.Generic;
using System.Linq;
using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Command
{
  public class ClientDisconnectedCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }
    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void Execute()
    {
      ushort clientId = (ushort)evt.data;

      if (!playerModel.userList.ContainsKey(clientId))
        return;
      if (playerModel.userList[clientId].lobbyCode==null)
        return;
      if (!lobbyModel.lobbies.ContainsKey(playerModel.userList[clientId].lobbyCode))
        return;
      
      lobbyModel.OnQuit(playerModel.userList[clientId].lobbyCode,clientId);

    }
  }
}