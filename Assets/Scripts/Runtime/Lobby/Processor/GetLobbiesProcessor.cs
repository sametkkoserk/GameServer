using Riptide;
using Runtime.Lobby.Enum;
using Runtime.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Lobby.Processor
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