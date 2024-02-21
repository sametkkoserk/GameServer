using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Processor
{
  public class FortifyProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel{ get; set; }

    public override void Execute()
    {
      MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;
      ushort clientId = messageReceivedVo.fromId;
      
      SendPacketWithLobbyCode<FortifyVo> vo = networkManager.GetData<SendPacketWithLobbyCode<FortifyVo>>(messageReceivedVo.message);
      vo.mainClass.clientId = clientId;

      DebugX.Log(DebugKey.MainGame, $"Fortify Processor. Fortifier City ID: {vo.mainClass.sourceCityId} ==> Target City ID: {vo.mainClass.targetCityId}");

      mainGameModel.mainGameManagerMediators[vo.lobbyCode].OnFortify(vo.mainClass);
    }
  }
}