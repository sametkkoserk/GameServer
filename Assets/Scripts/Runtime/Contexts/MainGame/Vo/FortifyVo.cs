using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class FortifyVo
  {
    [ProtoIgnore]
    public ushort clientId;
    
    [ProtoMember(1)]
    public CityVo sourceCityVo;
    
    [ProtoMember(2)]
    public CityVo targetCityVo;
  }
}