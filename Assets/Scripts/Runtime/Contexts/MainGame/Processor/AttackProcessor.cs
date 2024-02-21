using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Processor
{
  public class AttackProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel{ get; set; }

    public override void Execute()
    {
      MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;
      ushort clientId = messageReceivedVo.fromId;
      
      SendPacketWithLobbyCode<AttackVo> vo = networkManager.GetData<SendPacketWithLobbyCode<AttackVo>>(messageReceivedVo.message);
      vo.mainClass.clientId = clientId;

      DebugX.Log(DebugKey.MainGame, $"Attack Processor. Attacker ID: {vo.mainClass.attackerCityID} ==> Defender ID: {vo.mainClass.defenderCityID}");

      mainGameModel.mainGameManagerMediators[vo.lobbyCode].OnAttack(vo.mainClass);
    }
  }
}