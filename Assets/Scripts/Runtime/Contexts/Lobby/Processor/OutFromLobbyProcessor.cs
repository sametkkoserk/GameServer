using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Processor
{
  public class OutFromLobbyProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      ushort lobbyId = vo.fromId;
      string message = vo.message;
      
      OutFromLobbyVo outFromLobbyVo = networkManager.GetData<OutFromLobbyVo>(message);
      outFromLobbyVo.clientId = lobbyId;
      dispatcher.Dispatch(LobbyEvent.OutFromLobby,outFromLobbyVo);
    }
  }
}