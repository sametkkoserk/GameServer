using System.Collections.Generic;
using Lobby.Vo;
using MainGame.Vo;
using UnityEngine;

namespace MainGame.Model.MainGameModel
{
    public class MainGameModel : IMainGameModel
    {
        public LobbyVo createdLobbyVo { get; set; }

        [PostConstruct]
        public void OnPostConstruct()
        {
        }

        public Dictionary<int, CityVo> RandomMapGenerator()
        {
            int totalCity = 25;
            int xPos = 0;
            int zPos = 0;
            
            Dictionary<int, CityVo> cities = new Dictionary<int, CityVo>();

            for (int i = 0; i < totalCity; i++)
            {
                CityVo cityVo = new()
                {
                    isPlayable = Random.Range(0, 100) >= 15,
                    position = new Vector3(xPos, 0, zPos),
                    ID = xPos * 10 + zPos,
                    soldierCount = 0,
                    ownerID = Random.Range(0, 4),
                };
                
                cities.Add(cityVo.ID, cityVo);

                xPos++;

                if (xPos != 5) continue;
                xPos = 0;
                zPos++;
            }

            return cities;
        }
    }
}