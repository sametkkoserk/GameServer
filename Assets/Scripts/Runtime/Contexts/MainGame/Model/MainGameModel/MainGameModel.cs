using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Vo;
using UnityEngine;

namespace Runtime.Contexts.MainGame.Model.MainGameModel
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
            int totalCity = 57;
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
                    ownerID = Random.Range(0, 4),
                };
                
                cities.Add(cityVo.ID, cityVo);

                xPos++;

                if (xPos != 3) continue;
                xPos = 0;
                zPos++;
            }

            return cities;
        }
    }
}