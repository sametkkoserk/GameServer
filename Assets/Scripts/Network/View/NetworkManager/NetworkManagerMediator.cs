using System;
using Network.Enum;
using Network.Services.NetworkManager;
using Riptide;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Network.View.NetworkManager
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