using Runtime.Contexts.Lobby.Enum;
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

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      ushort fromId = vo.fromId;
      byte[] message = vo.message;
      string lobbyCode = networkManager.GetData<string>(message);

      JoinLobbyVo joinLobbyVo = new()
      {
        lobbyCode = lobbyCode,
        clientId = fromId
      };
      dispatcher.Dispatch(LobbyEvent.JoinLobby, joinLobbyVo);
    }
  }
}