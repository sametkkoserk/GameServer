using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.View.City;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

namespace Runtime.Contexts.MainGame.View.MainMap
{
  public class MainMapMediator : EventMediator
  {
    [Inject]
    public MainMapView view { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public void Start()
    {
      view.lobbyVo = mainGameModel.mapLobbyVos[0];
      mainGameModel.mapLobbyVos.Remove(view.lobbyVo);
      mainGameModel.mainMapMediators[view.lobbyVo.lobbyCode] = this;
    }

    public void SetMainGameManager()
    {
      view.mainGameManagerMediator = mainGameModel.mainGameManagerMediators[view.lobbyVo.lobbyCode];
    }
    
    public void OnPlayerSceneReady(string lobbycode,ushort clientId)
    {
      if (view.lobbyVo.clients[clientId].state==(ushort)ClientState.MainGameSceneReady)
        return;

      view.lobbyVo.readyCount++;
      view.lobbyVo.clients[clientId].state = (ushort)ClientState.MainGameSceneReady;
      
      if (view.lobbyVo.readyCount < view.lobbyVo.playerCount)
        return;
      view.lobbyVo.readyCount = 0;
      view.cities = mainGameModel.RandomMapGenerator(view.lobbyVo);

      for (int i = 0; i < view.cities.Count; i++)
      {
        int value = i;
        KeyValuePair<int, CityVo> city = view.cities.ElementAt(value);

        AsyncOperationHandle<GameObject> asyncOperation = Addressables.InstantiateAsync(MainGameKeys.City,new Vector3(25*value,3000,0),Quaternion.identity, gameObject.transform);
        asyncOperation.Completed += handle =>
        {
          if (handle.Status != AsyncOperationStatus.Succeeded) return;
          GameObject loadedObject = handle.Result;

          loadedObject.name = city.Key.ToString();
          loadedObject.transform.localPosition = city.Value.position.ToVector3();
          
          CityView cityView = loadedObject.GetComponent<CityView>();
          cityView.Init(city.Key, view.lobbyVo.lobbyCode);
          Debug.Log(view.lobbyVo.lobbyCode);
        };
      }
    }
    public void SendMap(int cityId, int neighborId)
    {
      view.GetSpecificCity(cityId).neighbors.Add(neighborId);

      if (!view.GetReadyCities().Contains(cityId))
        view.GetReadyCities().Add(cityId);

      if (view.GetReadyCities().Count != view.cities.Count)
        return;

      MapGeneratorVo mapGeneratorVo = new()
      {
        cityVos = view.cities,
        clients = view.lobbyVo.clients
      };
      
      dispatcher.Dispatch(MainGameEvent.SendMap, mapGeneratorVo);
    }
    
    public void OnGameStartCheck(string lobbyCode,ushort clientId)
    {
      if (view.lobbyVo.clients[clientId].state==(ushort)ClientState.MainGameStart)
        return;

      view.lobbyVo.readyCount++;
      view.lobbyVo.clients[clientId].state = (ushort)ClientState.MainGameStart;
      
      if (view.lobbyVo.readyCount < view.lobbyVo.playerCount)
        return;

      mainGameModel.managerLobbyVos.Add(view.lobbyVo);
      
      AsyncOperationHandle<GameObject> mainGameManager = Addressables.InstantiateAsync(MainGameKeys.MainGameManager, transform.parent.transform);
      mainGameManager.Completed += handle =>
      {
        if (handle.Status != AsyncOperationStatus.Succeeded) return;
        GameObject loadedObject = handle.Result;

        loadedObject.name = "Main Game Manager";
        loadedObject.transform.localPosition = new Vector3(0, 0, 0);
      };
    }
    
    public void OnClaimCity(ClaimCityVo claimCityVo, bool changeTurn = true)
    {
      if (!view.mainGameManagerMediator.IsClientTurn(claimCityVo.clientId)) return;

      if (!view.mainGameManagerMediator.IsEnoughFreeSoldier(claimCityVo.soldierCount, claimCityVo.clientId)) return;

      if (view.mainGameManagerMediator.IsCityNeutral(claimCityVo.cityId))
      {
        view.mainGameManagerMediator.ChangeCityOwner(claimCityVo.cityId, claimCityVo.clientId);
        view.mainGameManagerMediator.IncreaseCitySoldierCount(claimCityVo.cityId, claimCityVo.soldierCount, claimCityVo.clientId);
      }
      else if (!view.mainGameManagerMediator.IsCityOwnerTheClient(claimCityVo.cityId, claimCityVo.clientId))
      {
        return;
      }
      else if (view.mainGameManagerMediator.IsCityOwnerTheClient(claimCityVo.cityId, claimCityVo.clientId))
      {
        view.mainGameManagerMediator.IncreaseCitySoldierCount(claimCityVo.cityId, claimCityVo.soldierCount, claimCityVo.clientId);
      }
      
      SendPacketToLobbyVo<CityVo> sendPacketToLobbyVo = networkManager.SetSendPacketToLobbyVo(view.cities[claimCityVo.cityId], view.lobbyVo.clients);
      dispatcher.Dispatch(MainGameEvent.UpdateCity, sendPacketToLobbyVo);
      
      dispatcher.Dispatch(MainGameEvent.ChangePlayerFeature, view.mainGameManagerMediator.view.gameManagerVo.playerFeaturesVos[claimCityVo.clientId]);

      if (changeTurn)
        view.mainGameManagerMediator.ChangeTurn();
    }

    public void AssignCityRandomly(ushort queueId)
    {
      List<int> emptyCities = GetEmptyCities();
      int randomCity;

      if (emptyCities.Count == 0)
      {
        List<int> cities = view.mainGameManagerMediator.GetPlayerCities(queueId);

        randomCity = cities[Random.Range(0, cities.Count)];
      }
      else
      {
        randomCity = emptyCities[Random.Range(0, emptyCities.Count)];
      }

      ClaimCityVo claimCityVo = new()
      {
        soldierCount = 1,
        cityId = randomCity,
        clientId = queueId
      };
      
      OnClaimCity(claimCityVo, false);
    }

    public void OnArmingToCity(ArmingVo armingVo)
    {
      CityVo cityVo = view.cities[armingVo.cityID];
      
      if (view.mainGameManagerMediator.view.gameManagerVo.queueList.ElementAt(view.mainGameManagerMediator.view.gameManagerVo.queue) != armingVo.clientId)
        return;

      if (cityVo.ownerID != armingVo.clientId)
        return;

      cityVo.soldierCount += armingVo.soldierCount;
      view.cities[cityVo.ID] = cityVo;
      
      view.mainGameManagerMediator.ArmingToCity(armingVo);

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
  }
}