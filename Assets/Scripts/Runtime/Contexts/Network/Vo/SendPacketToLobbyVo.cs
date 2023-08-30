using System.Collections.Generic;
using ProtoBuf;
using Runtime.Contexts.Lobby.Vo;

namespace Runtime.Contexts.Network.Vo
{
  [ProtoContract]
  public class SendPacketToLobbyVo<T>
  {
    [ProtoMember(1)]
    public T mainClass;
    
    [ProtoIgnore]
    public Dictionary<ushort, ClientVo> clients;
  }
}