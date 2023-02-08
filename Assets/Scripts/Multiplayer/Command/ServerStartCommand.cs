using Multiplayer.Services.NetworkManager;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.impl;

namespace Multiplayer.Command
{
    public class ServerStartCommand : EventCommand
    {
        [Inject]
        public INetworkManagerService networkManager{get;set;}
        public override void Execute()
        {
            networkManager.Connect(8084, 10);
        }
    }
}