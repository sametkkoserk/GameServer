using Runtime.Contexts.MainGame.Command;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.Processor;
using Runtime.Contexts.MainGame.View.MainGameManager;
using Runtime.Contexts.MainGame.View.MainMap;
using Runtime.Contexts.MainGame.View.MainMapContainer;
using Runtime.Contexts.Network.Enum;
using Runtime.Modules.Core.GeneralContext;
using strange.extensions.context.api;
using UnityEngine;

namespace Runtime.Contexts.MainGame.Config
{
    public class MainGameContext : GeneralContext
    {
        public MainGameContext (MonoBehaviour view) : base(view)
        {
        }

        public MainGameContext (MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }
        
        protected override void mapBindings()
        {
            injectionBinder.Bind<IMainGameModel>().To<MainGameModel>().ToSingleton().CrossContext();
            
            mediationBinder.Bind<MainMapView>().To<MainMapMediator>();
            mediationBinder.Bind<MainMapContainerView>().To<MainMapContainerMediator>();
            mediationBinder.Bind<MainGameManagerView>().To<MainGameManagerMediator>();

            commandBinder.Bind(MainGameEvent.SendMap).To<SendMapCommand>();
            commandBinder.Bind(MainGameEvent.NextTurn).To<NextTurnCommand>();
            commandBinder.Bind(MainGameEvent.SendRemainingTime).To<SendRemainingTimeCommand>();
            commandBinder.Bind(MainGameEvent.ChangeGameState).To<ChangeGameStateCommand>();
            commandBinder.Bind(MainGameEvent.ChangePlayerAction).To<ChangePlayerActionCommand>();
            commandBinder.Bind(MainGameEvent.SetAllPermissionPlayersAction).To<SetAllPlayersActionReferenceCommand>();
            commandBinder.Bind(MainGameEvent.SendClaimedCity).To<SendClaimedCityCommand>();
            
            commandBinder.Bind(ClientToServerId.SceneReady).To<SceneReadyProccessor>();
            commandBinder.Bind(ClientToServerId.GameStart).To<GameStartProcessor>();
            commandBinder.Bind(ClientToServerId.ClaimCity).To<ClaimCityProcessor>();
        }
    }
}

