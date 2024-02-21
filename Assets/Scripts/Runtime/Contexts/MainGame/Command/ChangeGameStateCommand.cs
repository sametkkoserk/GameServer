using Riptide;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Command
{
  public class ChangeGameStateCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      SendPacketToLobbyVo<GameStateVo> gameStateVo = (SendPacketToLobbyVo<GameStateVo>)evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.GameStateChanged);
      message = networkManager.SetData(message, gameStateVo.mainClass);
      
      networkManager.SendToLobby(message, gameStateVo.clients);
    }
  }
}