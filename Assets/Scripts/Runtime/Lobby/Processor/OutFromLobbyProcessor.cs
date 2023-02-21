using Riptide;
using Runtime.Lobby.Enum;
using Runtime.Lobby.Vo;
using Runtime.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Lobby.Processor
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