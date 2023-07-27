using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MainGame.Command
{
  public class GameStartCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    public override void Execute()
    {
      MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;
      // ushort lobbyId = vo.fromId;
      GameStartVo gameStartVo = networkManager.GetData<GameStartVo>(messageReceivedVo.message);

      GameStartVo vo = new()
      {
        lobbyCode = gameStartVo.lobbyCode,
        gameStart = gameStartVo.gameStart
      };
      
      dispatcher.Dispatch(MainGameEvent.GameStart, vo);
    }
  }
}