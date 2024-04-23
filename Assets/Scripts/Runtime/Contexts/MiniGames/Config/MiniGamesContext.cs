using Runtime.Contexts.MiniGames.Command;
using Runtime.Contexts.MiniGames.Enum;
using Runtime.Contexts.MiniGames.Model.MiniGamesModel;
using Runtime.Contexts.MiniGames.Processor;
using Runtime.Contexts.MiniGames.View.MiniGame;
using Runtime.Contexts.MiniGames.View.MiniGameContainer;
using Runtime.Contexts.Network.Enum;
using Runtime.Modules.Core.Bundle.Model.BundleModel;
using Runtime.Modules.Core.Discord.View.Behaviour;
using Runtime.Modules.Core.GeneralContext;
using StrangeIoC.scripts.strange.extensions.context.api;
using UnityEngine;

namespace Runtime.Contexts.MiniGames.Config
{
    public class MiniGamesContext : GeneralContext
    {
        public MiniGamesContext (MonoBehaviour view) : base(view)
        {
        }

        public MiniGamesContext (MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }
        
        protected override void mapBindings()
        {
            base.mapBindings();
            
            injectionBinder.Bind<IMiniGamesModel>().To<MiniGamesModel>().ToSingleton().CrossContext();

            
            mediationBinder.Bind<MiniGameContainerView>().To<MiniGameContainerMediator>();
            mediationBinder.Bind<MiniGameView>().To<MiniGameMediator>();
            
            commandBinder.Bind(MiniGamesEvent.MiniGameCreated).To<MiniGameCreatedCommand>();
            commandBinder.Bind(MiniGamesEvent.SendState).To<SendStateCommand>();
            commandBinder.Bind(MiniGamesEvent.OnMiniGameEnded).To<MiniGameEndedCommand>();
            commandBinder.Bind(MiniGamesEvent.SendMap).To<SendMapCommand>();
            commandBinder.Bind(MiniGamesEvent.SendCreateMiniGameScene).To<SendCreateMiniGameSceneCommand>();



            commandBinder.Bind(ClientToServerId.ButtonClicked).To<ButtonClickedProcessor>();
            commandBinder.Bind(ClientToServerId.MiniGameSceneReady).To<MiniGameSceneReadyProcessor>();
            commandBinder.Bind(ClientToServerId.MiniGameCreated).To<MiniGameCreatedProcessor>();

        }
    }
}

