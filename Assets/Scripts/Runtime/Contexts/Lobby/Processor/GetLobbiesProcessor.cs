using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Processor
{
  public class GetLobbiesProcessor : EventCommand
  {

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      ushort fromId = vo.fromId;
      byte[] message = vo.message;
      
      dispatcher.Dispatch(LobbyEvent.SendLobbies,fromId);
    }
  }
}