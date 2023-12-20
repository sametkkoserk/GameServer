using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class FortifyVo
  {
    [ProtoIgnore]
    public ushort clientId;
    
    [ProtoMember(1)]
    public int sourceCityId;
    
    [ProtoMember(2)]
    public int targetCityId;

    [ProtoMember(3)]
    public int soldierCount;
  }
}