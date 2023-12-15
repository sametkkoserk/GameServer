using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class AttackVo
  {
    [ProtoIgnore]
    public ushort clientId;
    
    [ProtoMember(1)]
    public CityVo attackerCityVo;
    
    [ProtoMember(2)]
    public CityVo defenderCityVo;
  }
}