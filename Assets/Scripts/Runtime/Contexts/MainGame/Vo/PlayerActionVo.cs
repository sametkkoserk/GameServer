using System.Collections.Generic;
using ProtoBuf;
using Runtime.Contexts.MainGame.Enum;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class PlayerActionVo
  {
    [ProtoMember(1)]
    public Dictionary<ushort, List<PlayerActionKey>> playerActionKeys = new();
  }
}