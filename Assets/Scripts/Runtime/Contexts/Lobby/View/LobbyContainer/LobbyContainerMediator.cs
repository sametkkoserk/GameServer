using Runtime.Contexts.Lobby.Enum;
using strange.extensions.mediation.impl;
using UnityEngine.AddressableAssets;

namespace Runtime.Contexts.Lobby.View.LobbyContainer
{
  public class LobbyContainerMediator : EventMediator
  {
    [Inject]
    public LobbyContainerView view { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(LobbyEvent.CreateLobby, OnCreate);
    }

    private void OnCreate()
    {
      Addressables.InstantiateAsync(LobbyKey.Lobby, gameObject.transform);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(LobbyEvent.CreateLobby, OnCreate);
    }
  }
}