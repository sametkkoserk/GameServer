using Runtime.Main.Enum;
using strange.extensions.command.impl;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Main.Command
{
    public class LoadNetworkSceneCommand : EventCommand
    {
        public override void Execute()
        {
            Addressables.LoadSceneAsync(SceneKeys.NetworkScene,LoadSceneMode.Additive);
            Addressables.LoadSceneAsync(SceneKeys.LobbyScene, LoadSceneMode.Additive);
            Addressables.LoadSceneAsync(SceneKeys.MainGameScene, LoadSceneMode.Additive);
        }
    }
}