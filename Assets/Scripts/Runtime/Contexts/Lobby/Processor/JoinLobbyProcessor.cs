using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Processor
{
  public class JoinLobbyProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      
      ushort clientId = vo.fromId;
      byte[] message = vo.message;
      
      string lobbyCode = networkManager.GetData<string>(message);
      
      JoinLobbyVo joinLobbyVo = new()
      {
        lobbyCode = lobbyCode,
        clientId = clientId
      };

      if (!lobbyModel.lobbies.ContainsKey(lobbyCode))
      {
        dispatcher.Dispatch(LobbyEvent.LobbyIsClosed, clientId);
        return;
      }
      
      dispatcher.Dispatch(LobbyEvent.JoinLobby, joinLobbyVo);
      
      DebugX.Log(DebugKey.Handle, "Join Lobby Processor Handle. Lobby Code: ");
    }
  }
}