using System;
using Multiplayer.Enum;
using Multiplayer.Services.NetworkManager;
using strange.extensions.mediation.impl;

namespace Multiplayer.View.Deneme
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