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

namespace Runtime.Contexts.MainGame.Model.GameControllerModel
{
    public class GameControllerModel : IGameControllerModel
    {
        private IGameControllerModel _gameControllerModelImplementation;
        public Dictionary<string, MainMapMediator> mainMapMediators{ get; set; }
        public Dictionary<string, MainGameManagerMediator> mainGameMediators{ get; set; }
        
        [PostConstruct]
        public void OnPostConstruct()
        {
            mainGameMediators = new Dictionary<string, MainGameManagerMediator>();
            mainMapMediators = new Dictionary<string, MainMapMediator>();
        }

        public void OnPlayerActionEnd(string lobbyCode)
        {
            mainGameMediators[lobbyCode].ChangeTurn();
        }

        public void OnGameStart(string lobbyCode, GameStartVo gameStartVo)
        {
            mainMapMediators[lobbyCode].OnGameStartCheck(gameStartVo);
        }

        public void OnPlayerSceneReady(string lobbyCode, SceneReadyVo vo)
        {
            mainMapMediators[lobbyCode].OnPlayerSceneReady(vo);
        }

        public void OnClaimCity(string lobbyCode, SendPacketWithLobbyCode<CityVo> cityVo)
        {
            mainMapMediators[lobbyCode].OnClaimCity(cityVo);
        }
    }
}