using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Lobby.Command
{
  public class QuitFromLobbyDoneCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IPlayerModel playerModel { get; set; }


    public override void Execute()
    {
      QuitFromLobbyVo quitFromLobbyVo = (QuitFromLobbyVo)evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.QuitFromLobbyDone);
      message = networkManager.SetData(message, quitFromLobbyVo);

      networkManager.SendToLobby(message, quitFromLobbyVo.clients);
      
      networkManager.Server.Send(message, quitFromLobbyVo.id);

      playerModel.userList[quitFromLobbyVo.id].lobbyCode = null;

      DebugX.Log(DebugKey.Request, 
        $"Player ID: {quitFromLobbyVo.id}, Lobby Code: {quitFromLobbyVo.lobbyCode}, Process: Quit from lobby");
    }
  }
}