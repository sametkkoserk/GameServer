using Lobby.Enum;
using Lobby.Vo;
using Network.Vo;
using Riptide;
using strange.extensions.command.impl;

namespace Lobby.Processor
{
  public class GetLobbiesProcessor : EventCommand
  {

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      ushort fromId = vo.fromId;
      Message message = vo.message;
      
      dispatcher.Dispatch(LobbyEvent.SendLobbies,fromId);
    }
  }
}