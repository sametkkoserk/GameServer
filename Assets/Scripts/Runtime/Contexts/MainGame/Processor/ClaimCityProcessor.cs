using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.GameControllerModel;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MainGame.Processor
{
  public class ClaimCityProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IGameControllerModel gameControllerModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;
      ushort clientId = messageReceivedVo.fromId;
      
      SendPacketWithLobbyCode<CityVo> cityVo = networkManager.GetData<SendPacketWithLobbyCode<CityVo>>(messageReceivedVo.message);
      cityVo.mainClass.clientId = clientId;

      DebugX.Log(DebugKey.MainGame, $"Claim City Processor. City ID: {cityVo.mainClass.ID}");

      gameControllerModel.OnClaimCity(cityVo.lobbyCode, cityVo);
    }
  }
}