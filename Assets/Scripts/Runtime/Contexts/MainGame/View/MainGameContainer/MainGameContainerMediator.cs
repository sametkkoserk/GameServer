using Runtime.Contexts.MainGame.Enum;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Contexts.MainGame.View.MainMapContainer
{
  public class MainMapContainerMediator : EventMediator
  {
    [Inject]
    public MainMapContainerView view { get; set; }
    
    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.CreateMap, OnCreateMap);
    }

    public void OnCreateMap(IEvent payload)
    {
      string lobbyCode = (string)payload.data;
      AsyncOperationHandle<GameObject> mainMapObject = Addressables.InstantiateAsync(MainGameKeys.MainMap, gameObject.transform);

      mainMapObject.Completed += handle =>
      {
        if (handle.Status != AsyncOperationStatus.Succeeded) return;
        GameObject loadedObject = handle.Result;

        loadedObject.name = "Main Map: " + lobbyCode;
      };
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.CreateMap, OnCreateMap);
    }
  }
}