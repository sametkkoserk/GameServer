using Riptide;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Command
{
  public class ChangePlayerFeatureCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      PlayerFeaturesVo playerFeaturesVo = (PlayerFeaturesVo)evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.ChangePlayerFeature);

      message = networkManager.SetData(message, playerFeaturesVo);
      networkManager.Server.Send(message, playerFeaturesVo.clientId);
    }
  }
}