using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MiniGames.Enum;
using Runtime.Contexts.MiniGames.Model.MiniGamesModel;
using Runtime.Contexts.MiniGames.View.MiniGame;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Contexts.MiniGames.View.MiniGameContainer
{
    public class MiniGameContainerMediator : EventMediator
    {
        
        [Inject]
        public MiniGameContainerView view { get; set; }
    
        [Inject]
        public IMiniGamesModel miniGamesModel { get; set; }
        
        public override void OnRegister()
        {
            base.OnRegister();
            dispatcher.AddListener(MiniGamesEvent.OnCreateMiniGame,OnCreateMiniGame);
        }

        private void OnCreateMiniGame(IEvent payload)
        {
            LobbyVo lobbyVo = (LobbyVo)payload.data;
            string newGame = miniGamesModel.GetRandomMiniGame();
            Addressables.InstantiateAsync("MiniGame" , transform).Completed += handle =>
            {
                if (handle.Status==AsyncOperationStatus.Succeeded)
                {
                    MiniGameView miniGameView=handle.Result.GetComponent<MiniGameView>();
                    miniGameView.lobbyVo = lobbyVo;
                    miniGameView.miniGameKey = newGame;
                }
            };


        }

        public override void OnRemove()
        {
            dispatcher.RemoveListener(MiniGamesEvent.OnCreateMiniGame,OnCreateMiniGame);
        }
    }
}