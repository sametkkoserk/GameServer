using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

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
      
      SendPacketWithLobbyCode<int> vo = networkManager.GetData<SendPacketWithLobbyCode<int>>(messageReceivedVo.message);

      mainGameModel.mainGameManagerMediators[vo.lobbyCode].OnPass(clientId);

      DebugX.Log(DebugKey.MainGame, "Pass Processor.");

    }
  }
}