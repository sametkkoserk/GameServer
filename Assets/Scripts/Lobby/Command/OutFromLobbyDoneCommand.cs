
using Lobby.Vo;
using Network.Enum;
using Network.Services.NetworkManager;
using Riptide;
using strange.extensions.command.impl;

namespace Lobby.Command
{
  public class OutFromLobbyDoneCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      OutFromLobbyVo outFromLobbyVo = (OutFromLobbyVo)evt.data;
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.OutFromLobbyDone);
      message.AddUShort(outFromLobbyVo.inLobbyId);
      
      for (ushort i = 0; i < outFromLobbyVo.clients.Count; i++)
      {
        networkManager.Server.Send(message,outFromLobbyVo.clients[i].id);
      }
      networkManager.Server.Send(message,outFromLobbyVo.clientId);
    }
  }
}