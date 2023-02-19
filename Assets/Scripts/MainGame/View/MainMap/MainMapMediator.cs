using System;
using System.Collections.Generic;
using Lobby.Vo;
using MainGame.Enum;
using MainGame.Model.MainGameModel;
using MainGame.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace MainGame.View.MainMap
{
  public enum MainMapEvent
  {
    
  }
  public class MainMapMediator : EventMediator
  {
    [Inject]
    public MainMapView view { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    public override void OnRegister()
    {
      
    }

    public void Start()
    {
      view.lobbyVo = mainGameModel.createdLobbyVo;

      view.cities = mainGameModel.RandomMapGenerator();

      Debug.Log(view.cities);
      Debug.Log(view.lobbyVo.clients);
      
      MapGeneratorVo mapGeneratorVo = new()
      {
        cityVos = view.cities,
        clients = view.lobbyVo.clients
      };
      
      dispatcher.Dispatch(MainGameEvent.SendMap, mapGeneratorVo);
    }

    public override void OnRemove()
    {
    }
  }
}