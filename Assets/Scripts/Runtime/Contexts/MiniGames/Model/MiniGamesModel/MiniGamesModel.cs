using System;
using System.Collections.Generic;
using Runtime.Contexts.MiniGames.View.MiniGame;
using Runtime.Contexts.MiniGames.Vo;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = System.Random;

namespace Runtime.Contexts.MiniGames.Model.MiniGamesModel
{
    public class MiniGamesModel : IMiniGamesModel
    {
        public List<string> miniGames { get; set; }
        public Dictionary<string, MiniGameMediator> miniGameMediators { get; set; }
        
        [PostConstruct]
        public void OnPostConstruct()
        {
            miniGames = new List<string>() { "RaceGame" };
            miniGameMediators = new Dictionary<string, MiniGameMediator>();
        }
        
        public string GetRandomMiniGame()
        {
            Random rnd = new Random();

            int r = rnd.Next(miniGames.Count);
            return miniGames[r];
        }
        
        public void OnButtonClicked(ushort clientId, ClickedButtonsVo vo)
        {
            miniGameMediators[vo.lobbyCode].OnButtonClicked(clientId, vo);
        }

        public void MiniGameEnded(string lobbyCode)
        {
            miniGameMediators.Remove(lobbyCode);
        }

        public void OnMiniGameSceneReady(string lobbyCode, ushort clientId)
        {
            miniGameMediators[lobbyCode].OnSceneReady(clientId);
        }
        public void OnMiniGameCreated(string lobbyCode, ushort clientId)
        {
            miniGameMediators[lobbyCode].OnMiniGameCreated(clientId);
        }
        public void CreateNewGame(string key,Transform parent=null,Action<GameObject> action=null)
        {
            Addressables.InstantiateAsync(key, parent).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    handle.Result.transform.position = new Vector3(500 * miniGameMediators.Count, -1000, 0);
                    action.Invoke(handle.Result);
                }
            };
        }
    }
}