using Runtime.Contexts.Network.Command;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.View.NetworkManager;
using Runtime.Modules.Core.GeneralContext;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Runtime.Contexts.Network.Config
{
    public class NetworkContext : GeneralContext
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

