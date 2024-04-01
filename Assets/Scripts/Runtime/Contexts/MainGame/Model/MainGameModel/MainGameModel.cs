using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.View.MainGameManager;
using Runtime.Contexts.MainGame.View.MainMap;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace Runtime.Contexts.MainGame.Model.MainGameModel
{
    public class MainGameModel : IMainGameModel
    {
        public List<LobbyVo> mapLobbyVos { get; set; }
        
        public List<LobbyVo> managerLobbyVos { get; set; }

        public Dictionary<string, MainMapMediator> mainMapMediators { get; set; }
        
        public Dictionary<string, MainGameManagerMediator> mainGameManagerMediators { get; set; }
        
        
        [PostConstruct]
        public void OnPostConstruct()
        {
            mapLobbyVos = new List<LobbyVo>();
            managerLobbyVos = new List<LobbyVo>();
            mainGameManagerMediators = new Dictionary<string, MainGameManagerMediator>();
            mainMapMediators = new Dictionary<string, MainMapMediator>();
        }
        
        public Dictionary<int, CityVo> RandomMapGenerator(LobbyVo vo)
        {
            int totalCity = vo.playerCount * 5;
            int xPos = 0;
            int zPos = 0;
            
            Dictionary<int, CityVo> cities = new();

            for (int i = 0; i < totalCity; i++)
            {
                CityVo cityVo = new()
                {
                    isPlayable = Random.Range(0, 100) >= 15,
                    position = new Vector3Vo(new Vector3(xPos, 0, zPos)),
                    ID = i,
                    soldierCount = 0,
                    ownerID = 0,
                };
                
                cities.Add(cityVo.ID, cityVo);

                xPos++;

                if (xPos != 6) continue;
                xPos = 0;
                zPos++;
            }

            return cities;
        }

        public void MiniGameEnded(string lobbyCode, List<ushort> leaderBoard)
        {
            mainGameManagerMediators[lobbyCode].OnMiniGameEnded(leaderBoard);
        }
    }
}