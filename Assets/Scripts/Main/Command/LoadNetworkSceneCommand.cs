using Main.Enum;
using strange.extensions.command.impl;
using UnityEngine.AddressableAssets;

namespace Main.Command
{
    public class LoadNetworkSceneCommand : EventCommand
    {
        public override void Execute()
        {
            Addressables.LoadSceneAsync(SceneKeys.NetworkScene);
        }
    }
}