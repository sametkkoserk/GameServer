using System;
using System.Linq;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.Vo;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

namespace Runtime.Contexts.MainGame.View.MainMap
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
      dispatcher.AddListener(MainGameEvent.GameStart, OnGameStartCheck);

    }

    public void Start()
    {
      view.lobbyVo = mainGameModel.lobbyVos[0];
      mainGameModel.lobbyVos.Remove(view.lobbyVo);

      view.cities = mainGameModel.RandomMapGenerator(view.lobbyVo);

      MapGeneratorVo mapGeneratorVo = new()
      {
        cityVos = view.cities,
        clients = view.lobbyVo.clients
      };

      for (int i = 0; i < view.lobbyVo.playerCount; i++)
      {
        view.lobbyVo.clients.ElementAt(i).Value.ready = false;
      }
      view.lobbyVo.readyCount = 0;
      
      dispatcher.Dispatch(MainGameEvent.SendMap, mapGeneratorVo);
    }
    
    private void OnGameStartCheck(IEvent payload)
    {
      GameStartVo vo = (GameStartVo)payload.data;

      if (view.lobbyVo.lobbyCode != vo.lobbyCode)
        return;

      if (view.lobbyVo.clients[vo.clientId].ready)
        return;

      view.lobbyVo.readyCount++;
      view.lobbyVo.clients[vo.clientId].ready = true;
      
      if (view.lobbyVo.readyCount < view.lobbyVo.playerCount)
        return;
      
      dispatcher.Dispatch(MainGameEvent.SetQueue, view.lobbyVo);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.GameStart, OnGameStartCheck);
    }
  }
}