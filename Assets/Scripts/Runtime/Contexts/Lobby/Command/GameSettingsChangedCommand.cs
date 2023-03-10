using System.Collections.Generic;
using Riptide;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Command
{
  public class GameSettingsChangedCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo) evt.data;
      
      ushort fromId = vo.fromId;
      string message = vo.message;
      LobbySettingsVo lobbySettingsVo = networkManager.GetData<LobbySettingsVo>(message);

      Message newMessage = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.GameSettingsChanged);
      newMessage = networkManager.SetData(newMessage, lobbySettingsVo);

      for (ushort i = 0; i < lobbyModel.lobbies.Count; i++)
      {
        if (lobbyModel.lobbies[i].lobbyId != lobbySettingsVo.lobbyId) continue;
        for (ushort j = 0; j < lobbyModel.lobbies[i].clients.Count; j++)
        {
          networkManager.Server.Send(newMessage, lobbyModel.lobbies[i].clients[j].id);
        }
      }
    }
  }
}