using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Vo;
using UnityEngine;

namespace Runtime.Contexts.MainGame.Model.MainGameModel
{
    public class MainGameModel : IMainGameModel
    {
        public List<LobbyVo> lobbyVos { get; set; }

        [PostConstruct]
        public void OnPostConstruct()
        {
            lobbyVos = new List<LobbyVo>();
        }

        public Dictionary<int, CityVo> RandomMapGenerator(LobbyVo vo)
        {
            int totalCity = vo.playerCount * 10;
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
                    soldierCount = Random.Range(0, 100),
                    ownerID = -1,
                };
                
                cities.Add(cityVo.ID, cityVo);

                xPos++;

                if (xPos != 6) continue;
                xPos = 0;
                zPos++;
            }

            return cities;
        }
    }
}