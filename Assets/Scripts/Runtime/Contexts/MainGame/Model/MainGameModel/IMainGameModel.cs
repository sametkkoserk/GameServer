using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.View.MainGameManager;
using Runtime.Contexts.MainGame.View.MainMap;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Modules.Core.PromiseTool;

namespace Runtime.Contexts.MainGame.Model.MainGameModel
{
    public interface IMainGameModel
    {
      List<LobbyVo> mapLobbyVos { get; set; }
      
      List<LobbyVo> managerLobbyVos { get; set; }
      
      Dictionary<string, MainMapMediator> mainMapMediators { get; set; }
      
      Dictionary<string, MainGameManagerMediator> mainGameManagerMediators { get; set; }
      
      Dictionary<int, CityVo> RandomMapGenerator(LobbyVo vo);
      
      public int ClaimCitySoldierCount { get; set; }

      void MiniGameEnded(string LobbyCode, List<ushort> leaderBoard);
    }
}