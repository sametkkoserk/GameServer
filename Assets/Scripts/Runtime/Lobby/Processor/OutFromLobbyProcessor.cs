using Riptide;
using Runtime.Lobby.Enum;
using Runtime.Lobby.Vo;
using Runtime.Network.Services.NetworkManager;
using Runtime.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Lobby.Processor
{
  public class OutFromLobbyProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      ushort fromId = vo.fromId;
      string message = vo.message;
      
      OutFromLobbyVo outFromLobbyVo = networkManager.GetData<OutFromLobbyVo>(message);;
      outFromLobbyVo.clientId = fromId;
      dispatcher.Dispatch(LobbyEvent.OutFromLobby,outFromLobbyVo);
    }
  }
}