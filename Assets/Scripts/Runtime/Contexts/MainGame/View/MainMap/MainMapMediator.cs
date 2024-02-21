using System.Collections.Generic;
using System.Linq;
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

    public override void OnRegister()
    {
    }

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

      for (int i = 0; i < view.cities.Count; i++)
      {
        int value = i;
        
        AsyncOperationHandle<GameObject> mainGameManager = Addressables.InstantiateAsync(MainGameKeys.City, gameObject.transform);
        mainGameManager.Completed += handle =>
        {
          if (handle.Status != AsyncOperationStatus.Succeeded) return;
          GameObject loadedObject = handle.Result;

          KeyValuePair<int, CityVo> city = view.cities.ElementAt(value);
          loadedObject.name = city.Key.ToString();
          loadedObject.transform.localPosition = city.Value.position.ToVector3();
          
          CityView cityView = loadedObject.GetComponent<CityView>();
          cityView.Init(city.Key, view.lobbyVo.lobbyCode);
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
    
    public void OnGameStartCheck(GameStartVo vo)
    {
      if (view.lobbyVo.clients[vo.clientId].ready)
        return;

      view.lobbyVo.readyCount++;
      view.lobbyVo.clients[vo.clientId].ready = true;
      
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
      CityVo vo = view.cities[claimCityVo.cityId];
      
      if (vo.ownerID != 0)
        return;

      ushort queueId = view.mainGameManagerMediator.view.gameManagerVo.queueList.ElementAt(view.mainGameManagerMediator.view.gameManagerVo.queue);
      if (queueId != claimCityVo.clientId)
        return;

      if (claimCityVo.soldierCount > view.mainGameManagerMediator.view.gameManagerVo.playerFeaturesVos[claimCityVo.clientId].freeSoldierCount)
        return;
      
      vo.ownerID = claimCityVo.clientId;
      vo.soldierCount = claimCityVo.soldierCount + 1;
      view.cities[vo.ID] = vo;

      SendPacketToLobbyVo<CityVo> sendPacketToLobbyVo = networkManager.SetSendPacketToLobbyVo(vo, view.lobbyVo.clients);
      dispatcher.Dispatch(MainGameEvent.UpdateCity, sendPacketToLobbyVo);
      
      view.mainGameManagerMediator.view.gameManagerVo.playerFeaturesVos[claimCityVo.clientId].freeSoldierCount -= claimCityVo.soldierCount;
      dispatcher.Dispatch(MainGameEvent.ChangePlayerFeature, view.mainGameManagerMediator.view.gameManagerVo.playerFeaturesVos[claimCityVo.clientId]);

      if (changeTurn)
        view.mainGameManagerMediator.ChangeTurn();
    }

    public void AssignCityRandomly(ushort queueId)
    {
      List<int> emptyCities = GetEmptyCities();
      int randomEmptyCityId = emptyCities[Random.Range(0, emptyCities.Count)];

      ClaimCityVo claimCityVo = new()
      {
        soldierCount = 0,
        cityId = randomEmptyCityId,
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
    
    public override void OnRemove()
    {
    }
  }
}