using Riptide;
using Runtime.Lobby.Vo;
using Runtime.Network.Enum;
using Runtime.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Lobby.Command
{
  public class OutFromLobbyDoneCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      OutFromLobbyVo outFromLobbyVo = (OutFromLobbyVo)evt.data;
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.OutFromLobbyDone);
      message=networkManager.SetData(message,outFromLobbyVo.inLobbyId);
      
      for (ushort i = 0; i < outFromLobbyVo.clients.Count; i++)
      {
        networkManager.Server.Send(message,outFromLobbyVo.clients[i].id);
      }
      networkManager.Server.Send(message,outFromLobbyVo.clientId);
    }
  }
}