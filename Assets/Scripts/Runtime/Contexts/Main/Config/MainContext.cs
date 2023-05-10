using Runtime.Contexts.Main.Command;
using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.Main.Processor;
using Runtime.Contexts.Network.Enum;
using Runtime.Modules.Core.Discord.View.Behaviour;
using Runtime.Modules.Core.GeneralContext;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Runtime.Contexts.Main.Config
{
    public class MainContext : GeneralContext
    {
        public MainContext (MonoBehaviour view) : base(view)
        {
        }

        public MainContext (MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }
        
        protected override void mapBindings()
        {
            //Injection binding.
            //Map a mock model and a mock service, both as Singletons
            //injectionBinder.Bind<IExampleModel>().To<ExampleModel>().ToSingleton();
            //injectionBinder.Bind<INetworkManagerService>().To<NetworkManagerService>().ToSingleton();
            injectionBinder.Bind<IPlayerModel>().To<PlayerModel>().ToSingleton();
            //View/Mediator binding
            //This Binding instantiates a new ExampleMediator whenever as ExampleView
            //Fires its Awake method. The Mediator communicates to/from the View
            //and to/from the App. This keeps dependencies between the view and the app
            //separated.
            //mediationBinder.Bind<ExampleView>().To<ExampleMediator>();
            mediationBinder.Bind<DiscordBehaviourView>().To<DiscordBehaviourMediator>();

            //Event/Command binding
            //commandBinder.Bind(ExampleEvent.REQUEST_WEB_SERVICE).To<CallWebServiceCommand>();
            //The START event is fired as soon as mappings are complete.
            //Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
            commandBinder.Bind(ContextEvent.START).To<LoadNetworkSceneCommand>();

            commandBinder.Bind(MainEvent.CheckUserInfo).To<CheckUserInfoCommand>();
            
            commandBinder.Bind(ClientToServerId.Register).To<GetRegisterInfoProcessor>();
        }
    }
}

