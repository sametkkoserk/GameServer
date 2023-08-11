using System.Linq;
using Riptide;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MainGame.Command
{
  public class SendMapCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      MapGeneratorVo mapGeneratorVo = (MapGeneratorVo)evt.data;

      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.GameStartPreparations);
      message = networkManager.SetData(message, mapGeneratorVo);

      networkManager.SendToLobby(message, mapGeneratorVo.clients);
    }
  }
}