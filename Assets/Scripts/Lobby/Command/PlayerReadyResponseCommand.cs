using Lobby.Vo;
using Network.Enum;
using Network.Services.NetworkManager;
using Riptide;
using strange.extensions.command.impl;

namespace Lobby.Command
{
  public class PlayerReadyResponseCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      PlayerReadyVo playerReadyVo = (PlayerReadyVo)evt.data;
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.playerReadyResponse);
      message.AddUShort(playerReadyVo.inLobbyId);
      message.AddBool(playerReadyVo.startGame);
      for (int i = 0; i < playerReadyVo.clients.Count; i++)
      {
        networkManager.Server.Send(message,playerReadyVo.clients[i].id);
      }
    }
  }
}