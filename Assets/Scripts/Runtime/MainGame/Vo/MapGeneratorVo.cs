using System.Collections.Generic;
using Newtonsoft.Json;
using Runtime.Lobby.Vo;

namespace Runtime.MainGame.Vo
{
  public class MapGeneratorVo
  {
    public Dictionary<int, CityVo> cityVos;

    [JsonIgnore]
    public Dictionary<ushort,ClientVo> clients; 
  }
}