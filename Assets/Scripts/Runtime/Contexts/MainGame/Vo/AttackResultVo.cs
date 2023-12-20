using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class AttackResultVo
  {
    [ProtoMember(1)]
    public CityVo winnerCity;

    [ProtoMember(2)]
    public CityVo loserCity;

    [ProtoMember(3)]
    public bool isConquered;
  }
}