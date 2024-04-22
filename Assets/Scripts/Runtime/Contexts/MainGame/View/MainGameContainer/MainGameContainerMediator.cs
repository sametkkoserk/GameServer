using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
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
          localPosition = new Vector3(mainGameModel.mainGameManagerMediators.Count * 100, 1000, 0)
        }
      };
      Addressables.InstantiateAsync(MainGameKeys.MainMap, lobbyObject.transform).Completed += handle =>
      {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
          GameObject obj = handle.Result;
          obj.name = "Main Map";
          obj.transform.localPosition = new Vector3(0, 0, 0);
        }
      };
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.CreateMap, OnCreateMap);
    }
  }
}