using System.Collections.Generic;
using Lobby.Enum;
using Lobby.Model.LobbyModel;
using Lobby.Vo;
using MainGame.Enum;
using MainGame.Model.MainGameModel;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MainGame.View.MainMapContainer
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