using Riptide;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Command
{
  public class SendLobbiesCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }
    public override void Execute()
    {
      ushort clientId = (ushort)evt.data;
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.SendLobbies);
      message=networkManager.SetData(message,lobbyModel.lobbies);

      networkManager.Server.Send(message,clientId);
    }
  }
}