using Runtime.Contexts.MainGame.Command;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.View.MainMap;
using Runtime.Contexts.MainGame.View.MainMapContainer;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.Config
{
    public class MainGameContext : MVCSContext
    {
        public MainGameContext (MonoBehaviour view) : base(view)
        {
        }

        public MainGameContext (MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }
        
        protected override void mapBindings()
        {
            injectionBinder.Bind<IMainGameModel>().To<MainGameModel>().ToSingleton();
            //Injection binding.
            //Map a mock model and a mock service, both as Singletons
            //injectionBinder.Bind<INetworkManagerService>().To<NetworkManagerService>().ToSingleton();
            //View/Mediator binding
            //This Binding instantiates a new ExampleMediator whenever as ExampleView
            //Fires its Awake method. The Mediator communicates to/from the View
            //and to/from the App. This keeps dependencies between the view and the app
            //separated.
            mediationBinder.Bind<MainMapView>().To<MainMapMediator>();
            mediationBinder.Bind<MainMapContainerView>().To<MainMapContainerMediator>();


            //Event/Command binding
            //commandBinder.Bind(ExampleEvent.REQUEST_WEB_SERVICE).To<CallWebServiceCommand>();
            //The START event is fired as soon as mappings are complete.
            //Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
            // commandBinder.Bind(ContextEvent.START).To<CreateMapCommand>();
            commandBinder.Bind(MainGameEvent.SendMap).To<SendMapCommand>();
        }
    }
}

