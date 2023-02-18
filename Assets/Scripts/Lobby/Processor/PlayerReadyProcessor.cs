using Lobby.Enum;
using Lobby.Vo;
using Network.Vo;
using Riptide;
using strange.extensions.command.impl;

namespace Lobby.Processor
{
  public class PlayerReadyProcessor : EventCommand
  {

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      ushort fromId = vo.fromId;
      Message message = vo.message;
      
      ushort lobbyId = message.GetUShort();
      ushort inLobbyId = message.GetUShort();
      
      PlayerReadyVo playerReadyVo = new PlayerReadyVo()
      {
        clientId = fromId,
        lobbyId = lobbyId,
        inLobbyId=inLobbyId
      };
      dispatcher.Dispatch(LobbyEvent.PlayerReady,playerReadyVo);
    }
  }
}