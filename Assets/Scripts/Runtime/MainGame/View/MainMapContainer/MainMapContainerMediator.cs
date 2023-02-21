using Runtime.Lobby.Vo;
using Runtime.MainGame.Enum;
using Runtime.MainGame.Model.MainGameModel;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine.AddressableAssets;

namespace Runtime.MainGame.View.MainMapContainer
{
  public class MainMapContainerMediator : EventMediator
  {
    [Inject]
    public MainMapContainerView view { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }
    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.CreateMap, OnCreateMap);
    }

    public void OnCreateMap(IEvent payload)
    {
      Addressables.InstantiateAsync(MainGameKeys.MainMap, gameObject.transform);

      mainGameModel.createdLobbyVo = (LobbyVo)payload.data;
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.CreateMap, OnCreateMap);
    }
  }
}