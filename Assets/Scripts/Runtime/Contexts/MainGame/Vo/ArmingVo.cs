using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class ArmingVo
  {
    [ProtoIgnore]
    public ushort clientId;
    
    [ProtoMember(1)]
    public int cityID;

    [ProtoMember(2)]
    public int soldierCount;
  }
}