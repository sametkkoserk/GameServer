using System.Collections.Generic;
using Newtonsoft.Json;
using Runtime.Contexts.Lobby.Vo;

namespace Runtime.Contexts.MainGame.Vo
{
  public class MapGeneratorVo
  {
    public Dictionary<int, CityVo> cityVos;

    [JsonIgnore]
    public Dictionary<ushort,ClientVo> clients; 
  }
}