using Runtime.Network.Command;
using Runtime.Network.Services.NetworkManager;
using Runtime.Network.View.NetworkManager;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Runtime.Network.Config
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
            injectionBinder.Bind<INetworkManagerService>().To<NetworkManagerService>().ToSingleton().CrossContext();
            
            //Model
            
            
            //View/Mediator 
            mediationBinder.Bind<NetworkManagerView>().To<NetworkManagerMediator>();
            
            //Command
            commandBinder.Bind(ContextEvent.START).To<ServerStartCommand>();
            
            

            //Processor

        }
    }
}

