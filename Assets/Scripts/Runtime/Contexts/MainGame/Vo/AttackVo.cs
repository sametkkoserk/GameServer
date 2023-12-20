using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class AttackVo
  {
    [ProtoIgnore]
    public ushort clientId;
    
    [ProtoMember(1)]
    public int attackerCityID;
    
    [ProtoMember(2)]
    public int defenderCityID;
  }
}