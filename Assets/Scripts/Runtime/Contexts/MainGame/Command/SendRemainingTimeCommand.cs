using Riptide;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MainGame.Command
{
  public class SendRemainingTimeCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      SendPacketToLobbyVo<TurnVo> turnVo = (SendPacketToLobbyVo<TurnVo>)evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.RemainingTime);
      message = networkManager.SetData(message, turnVo.mainClass);
      
      networkManager.SendToLobby(message, turnVo.clients);
    }
  }
}