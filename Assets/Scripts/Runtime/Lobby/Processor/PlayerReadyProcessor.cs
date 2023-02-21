using Riptide;
using Runtime.Lobby.Enum;
using Runtime.Lobby.Vo;
using Runtime.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Lobby.Processor
{
  public class PlayerReadyProcessor : EventCommand
  {

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo) evt.data;
      
      ushort fromId = vo.fromId;
      Message message = vo.message;
      
      ushort lobbyId = message.GetUShort();
      ushort inLobbyId = message.GetUShort();
      
      PlayerReadyVo playerReadyVo = new()
      {
        lobbyId = lobbyId,
        inLobbyId = inLobbyId
      };
      
      dispatcher.Dispatch(LobbyEvent.PlayerReady,playerReadyVo);
    }
  }
}