using System.Collections.Generic;
using ProtoBuf;
using Runtime.Contexts.Lobby.Vo;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class MapGeneratorVo
  {
    [ProtoMember(1)]
    public Dictionary<int, CityVo> cityVos;

    [ProtoIgnoreAttribute]
    public Dictionary<ushort, ClientVo> clients; 
  }
}