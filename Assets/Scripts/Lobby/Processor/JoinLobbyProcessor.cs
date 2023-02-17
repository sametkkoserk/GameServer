using Lobby.Enum;
using Lobby.Vo;
using Network.Vo;
using Riptide;
using strange.extensions.command.impl;

namespace Lobby.Processor
{
  public class JoinLobbyProcessor : EventCommand
  {

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      ushort fromId = vo.fromId;
      Message message = vo.message;
      ushort lobbyId = message.GetUShort();
      
      JoinLobbyVo joinLobbyVo = new JoinLobbyVo();
      joinLobbyVo.lobbyId = lobbyId;
      joinLobbyVo.clientId=fromId;
      dispatcher.Dispatch(LobbyEvent.JoinLobby, joinLobbyVo);
    }
  }
}