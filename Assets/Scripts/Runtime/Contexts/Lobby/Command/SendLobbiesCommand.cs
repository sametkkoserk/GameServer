using System.Collections.Generic;
using System.Linq;
using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Command
{
  public class SendLobbiesCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      ushort clientId = (ushort)evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.SendLobbies);
      Dictionary<string,LobbyVo> lobbies=lobbyModel.lobbies.Where(lobby=>!lobby.Value.isStarted && !lobby.Value.isPrivate).ToDictionary(lobby => lobby.Key, lobby => lobby.Value);;
      message = networkManager.SetData(message, lobbies);
      networkManager.Server.Send(message, clientId);

      DebugX.Log(DebugKey.Request,
        $"Player ID: {clientId} Process: Lobbies");
    }
  }
}