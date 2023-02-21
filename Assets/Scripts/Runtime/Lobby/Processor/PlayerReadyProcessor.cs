using Riptide;
using Runtime.Lobby.Enum;
using Runtime.Lobby.Vo;
using Runtime.Network.Services.NetworkManager;
using Runtime.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Lobby.Processor
{
  public class PlayerReadyProcessor : EventCommand
  {

    [Inject]
    public INetworkManagerService networkManager { get; set; }
    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo) evt.data;
      
      ushort fromId = vo.fromId;
      string message = vo.message;
      PlayerReadyResponseVo playerReadyResponseVo = networkManager.GetData<PlayerReadyResponseVo>(message);;

      dispatcher.Dispatch(LobbyEvent.PlayerReady,playerReadyResponseVo);
    }
  }
}