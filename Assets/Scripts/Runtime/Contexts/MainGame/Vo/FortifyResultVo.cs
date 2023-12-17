using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class FortifyResultVo
  {
    [ProtoMember(1)]
    public CityVo sourceCity;

    [ProtoMember(2)]
    public CityVo targetCity;
  }
}