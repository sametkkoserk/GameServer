using Riptide;
using Riptide.Utils;
using UnityEngine;

namespace Network.Services.NetworkManager
{
    public class NetworkManagerService : INetworkManagerService 
    {
        public Server Server { get; private set; }

        [SerializeField] private ushort port;
        [SerializeField] private ushort maxClientCount;

        public void StartServer(ushort _port ,ushort _maxClientCount)
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
                Server.Update();
            }
        }

        public void OnQuit()
        {
            Server.Stop();
        }
    }
}
