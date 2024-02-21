using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Processor
{
  public class ClaimCityProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;
      ushort clientId = messageReceivedVo.fromId;
      
      SendPacketWithLobbyCode<ClaimCityVo> cityVo = networkManager.GetData<SendPacketWithLobbyCode<ClaimCityVo>>(messageReceivedVo.message);
      cityVo.mainClass.clientId = clientId;

      DebugX.Log(DebugKey.MainGame, $"Claim City Processor. City ID: {cityVo.mainClass.cityId}");

      mainGameModel.mainMapMediators[cityVo.lobbyCode].OnClaimCity(cityVo.mainClass);
    }
  }
}