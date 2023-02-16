using Network.Command;
using Network.Enum;
using Network.Processor;
using Network.Services.NetworkManager;
using Network.View.NetworkManager;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Network.Config
{
    public class NetworkContext : MVCSContext
    {
        public NetworkContext (MonoBehaviour view) : base(view)
        {
        }

        public NetworkContext (MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }
        
        protected override void mapBindings()
        {
            //Service
            injectionBinder.Bind<INetworkManagerService>().To<NetworkManagerService>().ToSingleton();
            
            //Model
            
            
            //View/Mediator 
            mediationBinder.Bind<NetworkManagerView>().To<NetworkManagerMediator>();
            
            //Command
            commandBinder.Bind(ContextEvent.START).To<ServerStartCommand>();
            
            
            commandBinder.Bind(NetworkEvent.SEND_RESPONSE).To<SendResponseCommand>();

            //Processor
            commandBinder.Bind(ClientToServerId.TEST).To<HandleMessageProcessor>();

        }
    }
}

