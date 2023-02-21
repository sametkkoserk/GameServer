using Runtime.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Network.Command
{
    public class ServerStartCommand : EventCommand
    {
        [Inject]
        public INetworkManagerService networkManagerService {get;set;}
        public override void Execute()
        {
            Retain();
            networkManagerService.StartServer(8083, 10);
            Release();
        }
    }
}