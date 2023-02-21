using System.Collections.Generic;
using Runtime.Lobby.Vo;
using Runtime.MainGame.Vo;

namespace Runtime.MainGame.Model.MainGameModel
{
    public interface IMainGameModel
    {
      LobbyVo createdLobbyVo { get; set; }

      Dictionary<int, CityVo> RandomMapGenerator();
    }
}