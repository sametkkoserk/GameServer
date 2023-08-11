using System.Collections.Generic;
using ProtoBuf;
using Runtime.Contexts.Lobby.Vo;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class TurnVo
  {
    [ProtoMember(1)]
    public ushort id;

    [ProtoMember(2)]
    public int remainingTime;
    
    [ProtoIgnore]
    public Dictionary<ushort, ClientVo> clientVos;
  }
}