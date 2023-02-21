using System.Collections.Generic;
using Runtime.Lobby.Vo;

namespace Runtime.MainGame.Vo
{
  public class MapGeneratorVo
  {
    public Dictionary<int, CityVo> cityVos;

    public Dictionary<ushort,ClientVo> clients; 
  }
}