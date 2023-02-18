using Lobby.Enum;
using Lobby.Model.LobbyModel;
using MainGame.Enum;
using strange.extensions.mediation.impl;
using UnityEngine.AddressableAssets;

namespace MainGame.View.MainMapContainer
{
  public class MainMapContainerMediator : EventMediator
  {
    [Inject]
    public MainMapContainerView view { get; set; }
    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.CreateMap, OnCreateMap);
    }

    public void OnCreateMap()
    {
      Addressables.InstantiateAsync(MainGameKeys.MainMap, gameObject.transform);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.CreateMap, OnCreateMap);
    }
  }
}