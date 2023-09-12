using System.Security.Cryptography.X509Certificates;
using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.GameControllerModel;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MainGame.Processor
{
  public class SceneReadyProccessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IGameControllerModel gameControllerModel { get; set; }
    
    public override void Execute()
    {
      MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;
      ushort id = messageReceivedVo.fromId;
      
      SceneReadyVo vo = networkManager.GetData<SceneReadyVo>(messageReceivedVo.message);

      vo.id = id;
      
      DebugX.Log(DebugKey.MainGame, $"Scene Ready processor");
      gameControllerModel.OnPlayerSceneReady(vo.lobbyCode,vo);
    }
  }
}