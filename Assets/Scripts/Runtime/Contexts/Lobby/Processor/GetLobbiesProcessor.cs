using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Processor
{
  public class GetLobbiesProcessor : EventCommand
  {

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      ushort fromId = vo.fromId;
      string message = vo.message;
      
      dispatcher.Dispatch(LobbyEvent.SendLobbies,fromId);
    }
  }
}