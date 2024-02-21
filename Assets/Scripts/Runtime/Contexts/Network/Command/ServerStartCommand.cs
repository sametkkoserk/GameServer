using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Network.Command
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