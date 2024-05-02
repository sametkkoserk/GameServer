using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Processor
{
  public class PassProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }
    public override void Execute()
    {
      MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;
      ushort clientId = messageReceivedVo.fromId;
      
      string lobbyCode = networkManager.GetData<string>(messageReceivedVo.message);

      mainGameModel.mainGameManagerMediators[lobbyCode].OnPass(clientId);

      DebugX.Log(DebugKey.MainGame, "Pass Processor.");

    }
  }
}