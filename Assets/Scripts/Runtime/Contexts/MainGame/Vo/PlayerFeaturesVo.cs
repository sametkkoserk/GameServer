using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class PlayerFeaturesVo
  {
    [ProtoMember(1)]
    public int freeSoldierCount;

    [ProtoIgnore]
    public ushort clientId;
  }
}