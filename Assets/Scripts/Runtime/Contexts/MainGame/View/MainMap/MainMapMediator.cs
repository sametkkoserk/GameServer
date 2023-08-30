using System.Linq;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
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
      dispatcher.AddListener(MainGameEvent.GameStart, OnGameStartCheck);
      dispatcher.AddListener(MainGameEvent.PlayerSceneReady, OnPlayerSceneReady);
      dispatcher.AddListener(MainGameEvent.ClaimCity, OnClaimCity);
    }

    public void Start()
    {
      view.lobbyVo = mainGameModel.mapLobbyVos[0];
      
      mainGameModel.mapLobbyVos.Remove(view.lobbyVo);
    }
    
    private void OnPlayerSceneReady(IEvent payload)
    {
      SceneReadyVo vo = (SceneReadyVo)payload.data;
      
      if (view.lobbyVo.lobbyCode != vo.lobbyCode)
        return;

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

      mainGameModel.managerLobbyVos.Add(view.lobbyVo);
      Addressables.InstantiateAsync(MainGameKeys.MainGameManager, transform);
    }
    
    public void OnClaimCity(IEvent payload)
    {
      SendPacketWithLobbyCode<CityVo> cityVo = (SendPacketWithLobbyCode<CityVo>) payload.data;

      if (view.lobbyVo.lobbyCode != cityVo.lobbyCode)
        return;

      if (cityVo.mainClass.ownerID != 0)
        return;

      cityVo.mainClass.ownerID = cityVo.mainClass.clientId;
      cityVo.mainClass.soldierCount = 1;

      view.cities[cityVo.mainClass.ID] = cityVo.mainClass;

      SendPacketToLobbyVo<CityVo> vo = networkManager.SetSendPacketToLobbyVo(cityVo.mainClass, view.lobbyVo.clients);
      
      dispatcher.Dispatch(MainGameEvent.SendClaimedCity, vo);
      dispatcher.Dispatch(MainGameEvent.PlayerActionEnded);
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
      dispatcher.RemoveListener(MainGameEvent.GameStart, OnGameStartCheck);
      dispatcher.RemoveListener(MainGameEvent.PlayerSceneReady, OnPlayerSceneReady);
      dispatcher.RemoveListener(MainGameEvent.ClaimCity, OnClaimCity);
    }
  }
}