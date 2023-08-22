using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Vo;

namespace Runtime.Contexts.MainGame.Model.MainGameModel
{
    public interface IMainGameModel
    {
      List<LobbyVo> mapLobbyVos { get; set; }
      List<LobbyVo> managerLobbyVos { get; set; }

      Dictionary<int, CityVo> RandomMapGenerator(LobbyVo vo);
    }
}