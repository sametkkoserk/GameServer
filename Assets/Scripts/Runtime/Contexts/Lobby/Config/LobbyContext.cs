using Runtime.Contexts.Lobby.Command;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Processor;
using Runtime.Contexts.Network.Enum;
using Runtime.Modules.Core.GeneralContext;
using StrangeIoC.scripts.strange.extensions.context.api;
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
            base.mapBindings();
            
            injectionBinder.Bind<ILobbyModel>().To<LobbyModel>().ToSingleton().CrossContext();

            
            commandBinder.Bind(LobbyEvent.JoinedToLobby).To<JoinedToLobbyCommand>();
            commandBinder.Bind(LobbyEvent.SendLobbies).To<SendLobbiesCommand>();
            commandBinder.Bind(LobbyEvent.QuitFromLobbyDone).To<QuitFromLobbyDoneCommand>();
            commandBinder.Bind(LobbyEvent.PlayerReadyResponse).To<PlayerReadyResponseCommand>();
            commandBinder.Bind(LobbyEvent.LobbyIsClosed).To<LobbyIsClosedCommand>();
            commandBinder.Bind(NetworkEvent.ClientDisconnected).To<ClientDisconnectedCommand>();

            
            commandBinder.Bind(ClientToServerId.CreateLobby).To<CreateLobbyProcessor>();
            commandBinder.Bind(ClientToServerId.GetLobbies).To<GetLobbiesProcessor>();
            commandBinder.Bind(ClientToServerId.JoinLobby).To<JoinLobbyProcessor>();
            commandBinder.Bind(ClientToServerId.QuitFromLobby).To<OutFromLobbyProcessor>();
            commandBinder.Bind(ClientToServerId.PlayerReady).To<PlayerReadyProcessor>();
            commandBinder.Bind(ClientToServerId.AddBot).To<AddBotProcessor>();
            commandBinder.Bind(ClientToServerId.GameSettingsChanged).To<GameSettingsChangedCommand>();
        }
    }
}

