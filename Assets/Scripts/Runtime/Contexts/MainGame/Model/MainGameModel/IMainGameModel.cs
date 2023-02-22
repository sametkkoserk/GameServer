using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Vo;

namespace Runtime.Contexts.MainGame.Model.MainGameModel
{
    public interface IMainGameModel
    {
      LobbyVo createdLobbyVo { get; set; }

      Dictionary<int, CityVo> RandomMapGenerator();
    }
}