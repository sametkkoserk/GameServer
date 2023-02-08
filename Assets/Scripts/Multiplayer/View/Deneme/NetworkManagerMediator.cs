using System;
using Multiplayer.Services.NetworkManager;
using strange.extensions.mediation.impl;

namespace Multiplayer.View.Deneme
{
    public class NetworkManagerMediator : EventMediator
    {
        [Inject]
        public INetworkManagerService networkManager{get;set;}
        private void FixedUpdate()
        {
            networkManager.Ticker();
        }

        private void OnApplicationQuit()
        {
            networkManager.OnQuit();
        }
    }
}