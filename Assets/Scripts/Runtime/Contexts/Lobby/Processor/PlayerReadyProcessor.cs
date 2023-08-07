using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Processor
{
  public class PlayerReadyProcessor : EventCommand
  {

    [Inject]
    public INetworkManagerService networkManager { get; set; }
    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo) evt.data;
      
      ushort fromId = vo.fromId;
      byte[] message = vo.message;
      PlayerReadyVo playerReadyVo = networkManager.GetData<PlayerReadyVo>(message);
      playerReadyVo.id = fromId;

      dispatcher.Dispatch(LobbyEvent.PlayerReady,playerReadyVo);
    }
  }
}