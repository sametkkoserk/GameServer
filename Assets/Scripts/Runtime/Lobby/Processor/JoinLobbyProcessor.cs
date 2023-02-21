using Riptide;
using Runtime.Lobby.Enum;
using Runtime.Lobby.Vo;
using Runtime.Network.Services.NetworkManager;
using Runtime.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Lobby.Processor
{
  public class JoinLobbyProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      ushort fromId = vo.fromId;
      string message = vo.message;
      ushort lobbyId = networkManager.GetData<ushort>(message);
      ;

      JoinLobbyVo joinLobbyVo = new JoinLobbyVo();
      joinLobbyVo.lobbyId = lobbyId;
      joinLobbyVo.clientId = fromId;
      dispatcher.Dispatch(LobbyEvent.JoinLobby, joinLobbyVo);
    }
  }
}