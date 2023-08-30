using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class TurnVo
  {
    [ProtoMember(1)]
    public ushort id;

    [ProtoMember(2)]
    public int remainingTime;
  }
}