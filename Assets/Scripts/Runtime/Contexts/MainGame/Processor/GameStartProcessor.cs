using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Processor
{
  public class GameStartProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }
    public override void Execute()
    {
      MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;
      ushort id = messageReceivedVo.fromId;
      
      GameStartVo gameStartVo = networkManager.GetData<GameStartVo>(messageReceivedVo.message);

      gameStartVo.clientId = id;

      DebugX.Log(DebugKey.MainGame, $"Game start processor");
      mainGameModel.mainMapMediators[gameStartVo.lobbyCode].OnGameStartCheck( gameStartVo);
    }
  }
}