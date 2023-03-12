using System.Linq;
using Riptide;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Command
{
  public class OutFromLobbyDoneCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      QuitFromLobbyVo quitFromLobbyVo = (QuitFromLobbyVo)evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.QuitFromLobbyDone);
      message = networkManager.SetData(message, quitFromLobbyVo.inLobbyId);

      for (ushort i = 0; i < quitFromLobbyVo.clients.Count; i++)
      {
        networkManager.Server.Send(message, quitFromLobbyVo.clients.ElementAt(i).Value.id);
      }

      networkManager.Server.Send(message, quitFromLobbyVo.clientId);
    }
  }
}