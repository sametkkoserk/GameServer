using System.Collections.Generic;
using Lobby.Vo;
using MainGame.Vo;

namespace MainGame.Model.MainGameModel
{
    public interface IMainGameModel
    {
      LobbyVo createdLobbyVo { get; set; }

      Dictionary<int, CityVo> RandomMapGenerator();
    }
}