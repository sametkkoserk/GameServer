using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Contexts.MainGame.View.MainGameContainer
{
  public class MainGameContainerMediator : EventMediator
  {
    [Inject]
    public MainGameContainerView view { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }
    
    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.CreateMap, OnCreateMap);
    }

    public void OnCreateMap(IEvent payload)
    {
      string lobbyCode = (string)payload.data;
      GameObject lobbyObject = new(lobbyCode)
      {
        transform =
        {
          parent = gameObject.transform,
          localPosition = new Vector3(mainGameModel.mainGameManagerMediators.Count * 20, 0, 0)
        }
      };

      AsyncOperationHandle<GameObject> mainMapObject = Addressables.InstantiateAsync(MainGameKeys.MainMap, lobbyObject.transform);

      mainMapObject.Completed += handle =>
      {
        if (handle.Status != AsyncOperationStatus.Succeeded) return;
        GameObject loadedObject = handle.Result;

        loadedObject.name = "Main Map";
        loadedObject.transform.localPosition = new Vector3(0, 0, 0);
      };
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.CreateMap, OnCreateMap);
    }
  }
}