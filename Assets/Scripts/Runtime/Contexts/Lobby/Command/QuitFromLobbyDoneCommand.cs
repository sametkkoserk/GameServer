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
      message = networkManager.SetData(message, quitFromLobbyVo);

      networkManager.SendToLobby(message, quitFromLobbyVo.clients);
      
      networkManager.Server.Send(message, quitFromLobbyVo.id);

      DebugX.Log(DebugKey.Request, 
        $"Player ID: {quitFromLobbyVo.id}, Lobby Code: {quitFromLobbyVo.lobbyCode}, Process: Quit from lobby");
    }
  }
}