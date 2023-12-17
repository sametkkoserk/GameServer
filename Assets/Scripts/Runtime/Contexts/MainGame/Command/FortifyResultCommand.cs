using Riptide;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MainGame.Command
{
  public class FortifyResultCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    public override void Execute()
    {
      SendPacketToLobbyVo<FortifyResultVo> attackResultVo = (SendPacketToLobbyVo<FortifyResultVo>)evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.Fortify);
      message = networkManager.SetData(message, attackResultVo.mainClass);
      
      networkManager.SendToLobby(message, attackResultVo.clients);
    }
  }
}