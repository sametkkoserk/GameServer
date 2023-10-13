using System.Collections.Generic;
using System.Linq;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.View.MainGameManager;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;

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

    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void OnRegister()
    {
    }

    public void Start()
    {
      view.lobbyVo = mainGameModel.mapLobbyVos[0];
      mainGameModel.mapLobbyVos.Remove(view.lobbyVo);
      mainGameModel.mainMapMediators[view.lobbyVo.lobbyCode] = this;
    }
    
    public void OnPlayerSceneReady(SceneReadyVo vo)
    {
      if (view.lobbyVo.clients[vo.id].ready)
        return;

      view.lobbyVo.readyCount++;
      view.lobbyVo.clients[vo.id].ready = true;
      
      if (view.lobbyVo.readyCount < view.lobbyVo.playerCount)
        return;
      
      for (int i = 0; i < view.lobbyVo.clients.Count; i++)
      {
        view.lobbyVo.clients.ElementAt(i).Value.ready = false;
      }
      view.lobbyVo.readyCount = 0;
      
      view.cities = mainGameModel.RandomMapGenerator(view.lobbyVo);

      MapGeneratorVo mapGeneratorVo = new()
      {
        cityVos = view.cities,
        clients = view.lobbyVo.clients
      };
      
      LoadingPlayerActions();

      dispatcher.Dispatch(MainGameEvent.SendMap, mapGeneratorVo);
    }
    
    public void OnGameStartCheck(GameStartVo vo)
    {
      if (view.lobbyVo.clients[vo.clientId].ready)
        return;

      view.lobbyVo.readyCount++;
      view.lobbyVo.clients[vo.clientId].ready = true;
      
      if (view.lobbyVo.readyCount < view.lobbyVo.playerCount)
        return;

      mainGameModel.managerLobbyVos.Add(view.lobbyVo);
      Addressables.InstantiateAsync(MainGameKeys.MainGameManager, transform);
    }
    
    public void OnClaimCity(CityVo cityVo, bool changeTurn = true)
    {
      MainGameManagerMediator mainGameMediator = mainGameModel.mainGameMediators[view.lobbyVo.lobbyCode];
      if (cityVo.ownerID != 0)
        return;

      if (mainGameMediator.view.gameManagerVo.queueList.ElementAt(mainGameMediator.view.gameManagerVo.queue) != cityVo.clientId)
        return;

      cityVo.ownerID = cityVo.clientId;
      cityVo.soldierCount = 1;

      view.cities[cityVo.ID] = cityVo;

      SendPacketToLobbyVo<CityVo> vo = networkManager.SetSendPacketToLobbyVo(cityVo, view.lobbyVo.clients);
      
      dispatcher.Dispatch(MainGameEvent.UpdateCity, vo);

      if (changeTurn)
        mainGameModel.mainGameMediators[view.lobbyVo.lobbyCode].ChangeTurn();
    }

    public void AssignCityRandomly(ushort queueId)
    {
      List<int> emptyCities = GetEmptyCities(); 

      int randomEmptyCityId = emptyCities[Random.Range(0, emptyCities.Count)];

      CityVo cityVo = view.cities[randomEmptyCityId];
      cityVo.clientId = queueId;
      
      OnClaimCity(cityVo, false);
    }

    public void OnArmingToCity(ArmingVo armingVo)
    {
      MainGameManagerMediator mainGameMediator = mainGameModel.mainGameMediators[view.lobbyVo.lobbyCode];

      if (armingVo.cityVo.ownerID != armingVo.clientId)
        return;

      if (mainGameMediator.view.gameManagerVo.queueList.ElementAt(mainGameMediator.view.gameManagerVo.queue) != armingVo.clientId)
        return;

      CityVo cityVo = view.cities[armingVo.cityVo.ID];
      if (cityVo.ownerID != armingVo.clientId)
        return;

      cityVo.soldierCount += armingVo.soldierCount;
      view.cities[cityVo.ID] = cityVo;
      
      mainGameMediator.ArmingToCity(armingVo);

      SendPacketToLobbyVo<CityVo> vo = networkManager.SetSendPacketToLobbyVo(cityVo, view.lobbyVo.clients);
      dispatcher.Dispatch(MainGameEvent.UpdateCity, vo);
    }

    public List<int> GetEmptyCities()
    {
      List<int> emptyCities = new();
      
      for (int i = 0; i < view.cities.Count; i++)
      {
        CityVo city = view.cities.ElementAt(i).Value;
        
        if (city.ownerID == 0 && city.isPlayable)
          emptyCities.Add(city.ID);
      }
      
      return emptyCities;
    }
    
    private void LoadingPlayerActions()
    {
      if (!mainGameModel.loaded)
      {
        mainGameModel.Init().Then(SetPlayerActionsReferenceList);
        return;
      }
      
      SetPlayerActionsReferenceList();
    }

    private void SetPlayerActionsReferenceList()
    {
      PlayerActionPermissionReferenceSendVo vo = new()
      {
        allPlayerActionsReferenceList = mainGameModel.necessaryKeysForActions,
        clients = view.lobbyVo.clients
      };

      dispatcher.Dispatch(MainGameEvent.SetAllPermissionPlayersAction, vo);
    }

    public override void OnRemove()
    {
    }
  }
}