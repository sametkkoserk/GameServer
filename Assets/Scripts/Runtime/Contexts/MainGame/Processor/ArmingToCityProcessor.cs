using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MainGame.Processor
{
  public class ArmingToCityProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel{ get; set; }

    public override void Execute()
    {
      MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;
      ushort clientId = messageReceivedVo.fromId;
      
      SendPacketWithLobbyCode<ArmingVo> vo = networkManager.GetData<SendPacketWithLobbyCode<ArmingVo>>(messageReceivedVo.message);
      vo.mainClass.clientId = clientId;

      DebugX.Log(DebugKey.MainGame, $"Arming to City Processor. City ID: {vo.mainClass.cityVo.ID}");

      mainGameModel.mainMapMediators[vo.lobbyCode].OnArmingToCity(vo.mainClass);
    }
  }
}