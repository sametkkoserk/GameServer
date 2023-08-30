using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Main.Model.PlayerModel;
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
    public IPlayerModel playerModel { get; set; }
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      
      ushort fromId = vo.fromId;
      byte[] message = vo.message;
      
      string lobbyCode = networkManager.GetData<string>(message);
      
      if (!lobbyModel.lobbies.ContainsKey(lobbyCode))
      {
        dispatcher.Dispatch(LobbyEvent.LobbyIsClosed, fromId);
        return;
      }
      
      ClientVo clientVo = new()
      {
        id = fromId,
        playerColor = new PlayerColorVo(lobbyModel.ColorGenerator()),
        userName = playerModel.userList[fromId].username
      };
      lobbyModel.OnJoin(lobbyCode,clientVo);
      
      DebugX.Log(DebugKey.Handle, "Join Lobby Processor Handle. Lobby Code: ");
    }
  }
}