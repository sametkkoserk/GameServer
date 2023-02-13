using Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Network.Command
{
    public class ServerStartCommand : EventCommand
    {
        [Inject]
        public INetworkManagerService networkManager{get;set;}
        public override void Execute()
        {
            networkManager.StartServer(8084, 10);
        }
    }
}