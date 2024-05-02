using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MiniGames.Enum;
using Runtime.Contexts.MiniGames.Model.MiniGamesModel;
using Runtime.Contexts.MiniGames.View.MiniGame;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
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
            miniGamesModel.CreateNewGame("MiniGame",transform, obj =>
            {
                MiniGameView miniGameView=obj.GetComponent<MiniGameView>();
                miniGameView.lobbyVo = lobbyVo;
                miniGameView.miniGameKey = newGame;
                miniGameView.Init();
            } );
        }

        public override void OnRemove()
        {
            dispatcher.RemoveListener(MiniGamesEvent.OnCreateMiniGame,OnCreateMiniGame);
        }
    }
}