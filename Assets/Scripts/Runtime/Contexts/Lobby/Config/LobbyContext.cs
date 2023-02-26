using Runtime.Contexts.Lobby.Command;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Processor;
using Runtime.Contexts.Lobby.View.Lobby;
using Runtime.Contexts.Lobby.View.LobbyContainer;
using Runtime.Contexts.Network.Enum;
using Runtime.Modules.Core.GeneralContext;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Config
{
    public class LobbyContext : GeneralContext
    {
        public LobbyContext (MonoBehaviour view) : base(view)
        {
        }

        public LobbyContext (MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }
        
        protected override void mapBindings()
        {
            //Injection binding.
            //Map a mock model and a mock service, both as Singletons
            injectionBinder.Bind<ILobbyModel>().To<LobbyModel>().ToSingleton();
            //injectionBinder.Bind<INetworkManagerService>().To<NetworkManagerService>().ToSingleton();
            //View/Mediator binding
            //This Binding instantiates a new ExampleMediator whenever as ExampleView
            //Fires its Awake method. The Mediator communicates to/from the View
            //and to/from the App. This keeps dependencies between the view and the app
            //separated.
            mediationBinder.Bind<LobbyView>().To<LobbyMediator>();
            mediationBinder.Bind<LobbyContainerView>().To<LobbyContainerMediator>();
            //Event/Command binding
            //commandBinder.Bind(ExampleEvent.REQUEST_WEB_SERVICE).To<CallWebServiceCommand>();
            //The START event is fired as soon as mappings are complete.
            //Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
            //commandBinder.Bind(ContextEvent.START).To<>();
            commandBinder.Bind(LobbyEvent.JoinedToLobby).To<JoinedToLobbyCommand>();
            commandBinder.Bind(LobbyEvent.SendLobbies).To<SendLobbiesCommand>();
            commandBinder.Bind(LobbyEvent.OutFromLobbyDone).To<OutFromLobbyDoneCommand>();
            commandBinder.Bind(LobbyEvent.PlayerReadyResponse).To<PlayerReadyResponseCommand>();

            
            commandBinder.Bind(ClientToServerId.CreateLobby).To<CreateLobbyProcessor>();
            commandBinder.Bind(ClientToServerId.GetLobbies).To<GetLobbiesProcessor>();
            commandBinder.Bind(ClientToServerId.JoinLobby).To<JoinLobbyProcessor>();
            commandBinder.Bind(ClientToServerId.OutFromLobby).To<OutFromLobbyProcessor>();
            commandBinder.Bind(ClientToServerId.PlayerReady).To<PlayerReadyProcessor>();
            
            
        }
    }
}

