using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Processor
{
  public class OutFromLobbyProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      ushort clientId = vo.fromId;
      byte[] message = vo.message;
      
      QuitFromLobbyVo quitFromLobbyVo = networkManager.GetData<QuitFromLobbyVo>(message);
      quitFromLobbyVo.id = clientId;
      
      dispatcher.Dispatch(LobbyEvent.QuitFromLobby,quitFromLobbyVo);
      
      DebugX.Log(DebugKey.Request, 
        $"Player ID: {quitFromLobbyVo.id}, Lobby ID: {quitFromLobbyVo.lobbyCode}, Process: Quit Request Handled.");
    }
  }
}