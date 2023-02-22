using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.mediation.impl;

namespace Runtime.Contexts.Network.View.NetworkManager
{
    public class NetworkManagerMediator : EventMediator
    {
        [Inject]
        public INetworkManagerService networkManagerService {get;set;}
        
        private void FixedUpdate()
        {
            networkManagerService.Ticker();
        }

        private void OnApplicationQuit()
        {
            networkManagerService.OnQuit();
        }
    }
}