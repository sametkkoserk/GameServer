using Lobby.Enum;
using Lobby.Vo;
using Network.Vo;
using Riptide;
using strange.extensions.command.impl;

namespace Lobby.Processor
{
  public class OutFromLobbyProcessor : EventCommand
  {

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      ushort fromId = vo.fromId;
      Message message = vo.message;
      ushort lobbyId = message.GetUShort();
      ushort inLobbyId = message.GetUShort();

      OutFromLobbyVo outFromLobbyVo = new OutFromLobbyVo()
      {
        clientId = fromId,
        lobbyId = lobbyId,
        inLobbyId=inLobbyId
      };
      dispatcher.Dispatch(LobbyEvent.OutFromLobby,outFromLobbyVo);
    }
  }
}