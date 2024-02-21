using Runtime.Contexts.Main.Enum;
using StrangeIoC.scripts.strange.extensions.command.impl;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Contexts.Main.Command
{
    public class LoadNetworkSceneCommand : EventCommand
    {
        public override void Execute()
        {
            Addressables.LoadSceneAsync(SceneKeys.NetworkScene, LoadSceneMode.Additive).Completed += handle =>
            {
                Addressables.LoadSceneAsync(SceneKeys.LobbyScene, LoadSceneMode.Additive).Completed += handle2 =>
                {
                    Addressables.LoadSceneAsync(SceneKeys.MainGameScene, LoadSceneMode.Additive).Completed += handle3 =>
                    {
                        Addressables.LoadSceneAsync(SceneKeys.MiniGamesScene, LoadSceneMode.Additive);
                    };
                };
            };
        }
    }
}