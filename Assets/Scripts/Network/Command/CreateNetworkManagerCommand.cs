using Main.Enum;
using Network.Enum;
using Network.Services.NetworkManager;
using strange.extensions.command.impl;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Network.Command
{
    public class CreateNetworkManagerCommand : EventCommand
    {

        public override void Execute()
        {
            Addressables.InstantiateAsync(NetworkKeys.NetworkManager);
        }
    }
}