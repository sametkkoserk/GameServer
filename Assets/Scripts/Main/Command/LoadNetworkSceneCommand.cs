using Main.Enum;
using strange.extensions.command.impl;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Main.Command
{
    public class LoadNetworkSceneCommand : EventCommand
    {
        public override void Execute()
        {
            Addressables.LoadSceneAsync(SceneKeys.NetworkScene,LoadSceneMode.Additive);
        }
    }
}