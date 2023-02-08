using Multiplayer.Command;
using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

namespace Multiplayer.Services.NetworkManager
{
    public class NetworkManagerService : INetworkManagerService 
    {
        public Server Server { get; private set; }

        [SerializeField] private ushort port;
        [SerializeField] private ushort maxClientCount;

        public void Connect(ushort _port ,ushort _maxClientCount)
        {
            port = _port;
            maxClientCount = _maxClientCount;
            
            RiptideLogger.Initialize(Debug.Log,Debug.Log,Debug.LogWarning,Debug.LogError,false);
            Server = new Server();
            Server.Start(port,maxClientCount);
        }

        public void Ticker()
        {
            if (Server != null)
            {
                Server.Tick();
            }
        }

        public void OnQuit()
        {
            Server.Stop();
        }
    }
}
