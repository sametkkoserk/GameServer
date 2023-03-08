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
      string message = messageReceivedVo.message;
      GameStartVo gameStartVo = networkManager.GetData<GameStartVo>(message);

      GameStartVo vo = new()
      {
        lobbyId = gameStartVo.lobbyId,
        gameStart = gameStartVo.gameStart
      };
      
      dispatcher.Dispatch(MainGameEvent.GameStart, vo);
    }
  }
}