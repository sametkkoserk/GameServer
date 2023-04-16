using System.Linq;
using Riptide;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Processor
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
      byte[] message = vo.message;
      LobbySettingsVo lobbySettingsVo = networkManager.GetData<LobbySettingsVo>(message);

      Message newMessage = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.GameSettingsChanged);
      newMessage = networkManager.SetData(newMessage, lobbySettingsVo);

      LobbyVo lobbyVo = lobbyModel.lobbies[lobbySettingsVo.lobbyId];
      lobbyModel.lobbies[lobbySettingsVo.lobbyId].lobbySettingsVo = lobbySettingsVo;

      for (int i = 0; i < lobbyVo.clients.Count; i++)
        networkManager.Server.Send(newMessage, lobbyVo.clients.ElementAt(i).Value.id);
    }
  }
}