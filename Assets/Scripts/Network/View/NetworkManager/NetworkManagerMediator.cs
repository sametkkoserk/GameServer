using Network.Enum;
using Network.Services.NetworkManager;
using strange.extensions.mediation.impl;

namespace Network.View.NetworkManager
{
    public class NetworkManagerMediator : EventMediator
    {
        public bool control;
        [Inject]
        public INetworkManagerService networkManager{get;set;}
        private void FixedUpdate()
        {
            networkManager.Ticker();
            if (control)
            {
                dispatcher.Dispatch(NetworkEvent.SendResponse);
                control = false;
            }
        }

        private void OnApplicationQuit()
        {
            networkManager.OnQuit();
        }
    }
}