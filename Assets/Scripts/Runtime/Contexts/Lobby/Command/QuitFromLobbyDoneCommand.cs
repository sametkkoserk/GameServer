using System.Linq;
using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Command
{
  public class QuitFromLobbyDoneCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      QuitFromLobbyVo quitFromLobbyVo = (QuitFromLobbyVo)evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.QuitFromLobbyDone);
      message = networkManager.SetData(message, quitFromLobbyVo.inLobbyId);

      networkManager.SendToLobby(message,quitFromLobbyVo.clients);

      networkManager.Server.Send(message, quitFromLobbyVo.clientId);
      
      DebugX.Log(DebugKey.Request, 
        $"Player ID: {quitFromLobbyVo.clientId} Player's Lobby ID: {quitFromLobbyVo.inLobbyId}, Lobby ID: {quitFromLobbyVo.lobbyId} Process: Quit from lobby");

    }
  }
}