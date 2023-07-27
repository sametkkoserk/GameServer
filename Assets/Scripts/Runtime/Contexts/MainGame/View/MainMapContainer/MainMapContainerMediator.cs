using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine.AddressableAssets;

namespace Runtime.Contexts.MainGame.View.MainMapContainer
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
      mainGameModel.createdLobbyVo = (LobbyVo)payload.data;

      if (mainGameModel.createdLobbyVo.readyCount == mainGameModel.createdLobbyVo.playerCount)
      {
        Addressables.InstantiateAsync(MainGameKeys.MainMap, gameObject.transform);
      }
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.CreateMap, OnCreateMap);
    }
  }
}