using Runtime.Contexts.Network.Enum;
using strange.extensions.command.impl;
using UnityEngine.AddressableAssets;

namespace Runtime.Contexts.Network.Command
{
    public class CreateNetworkManagerCommand : EventCommand
    {
        public override void Execute()
        {
            Addressables.InstantiateAsync(NetworkKeys.NetworkManager);
        }
    }
}