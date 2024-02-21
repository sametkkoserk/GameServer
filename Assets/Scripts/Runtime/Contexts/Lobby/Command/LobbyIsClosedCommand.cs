using Riptide;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Lobby.Command
{
  public class LobbyIsClosedCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      ushort clientId = (ushort)evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.LobbyIsClosed);
      
      message = networkManager.SetData(message, true);
      networkManager.Server.Send(message, clientId);
    }
  }
}