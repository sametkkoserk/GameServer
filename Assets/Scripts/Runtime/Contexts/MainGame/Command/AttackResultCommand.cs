using Riptide;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Command
{
  public class AttackResultCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    public override void Execute()
    {
      SendPacketToLobbyVo<AttackResultVo> attackResultVo = (SendPacketToLobbyVo<AttackResultVo>)evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.Attack);
      message = networkManager.SetData(message, attackResultVo.mainClass);
      
      networkManager.SendToLobby(message, attackResultVo.clients);
    }
  }
}