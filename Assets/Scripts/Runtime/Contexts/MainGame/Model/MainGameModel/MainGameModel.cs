using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.ScriptableObjects;
using Runtime.Contexts.MainGame.View.MainGameManager;
using Runtime.Contexts.MainGame.View.MainMap;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Vo;
using Runtime.Modules.Core.Bundle.Model.BundleModel;
using Runtime.Modules.Core.PromiseTool;
using UnityEngine;

namespace Runtime.Contexts.MainGame.Model.MainGameModel
{
    public class MainGameModel : IMainGameModel
    {
        public List<LobbyVo> mapLobbyVos { get; set; }
        
        public List<LobbyVo> managerLobbyVos { get; set; }

        public Dictionary<PlayerActionKey, PlayerActionPermissionReferenceVo> necessaryKeysForActions { get; set; }

        public Dictionary<string, MainMapMediator> mainMapMediators { get; set; }
        
        public Dictionary<string, MainGameManagerMediator> mainGameMediators { get; set; }
        
        public bool loaded { get; set; }

        [Inject]
        public IBundleModel bundleModel { get; set; }

        [PostConstruct]
        public void OnPostConstruct()
        {
            mapLobbyVos = new List<LobbyVo>();
            managerLobbyVos = new List<LobbyVo>();
            necessaryKeysForActions = new Dictionary<PlayerActionKey, PlayerActionPermissionReferenceVo>();
            mainGameMediators = new Dictionary<string, MainGameManagerMediator>();
            mainMapMediators = new Dictionary<string, MainMapMediator>();
        }
        
        public IPromise Init()
        {
            Promise promise = new();

            bundleModel.LoadAssetAsync<PlayerActionData>("PlayerActionData").Then(data =>
            {
                necessaryKeysForActions = new Dictionary<PlayerActionKey, PlayerActionPermissionReferenceVo>();

                foreach (PlayerActionPermissionReferenceVo playerActionVo in data.playerActionNecessaryVos)
                    necessaryKeysForActions.Add(playerActionVo.playerActionKey, playerActionVo);

                loaded = true;
                promise.Resolve();
            }).Catch(promise.Reject);

            return promise;
        }

        public Dictionary<int, CityVo> RandomMapGenerator(LobbyVo vo)
        {
            int totalCity = vo.playerCount * 3;
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
    }
}