using System.Threading.Tasks;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.City
{
  public class CityMediator : EventMediator
  {
    [Inject]
    public CityView view { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    public override void OnRegister()
    {
      Init();
    }

    private async Task Init()
    {
      await WaitAsyncOperations(0.5f);
    }

    public void OnTriggerEnter(Collider other)
    {
      CityView cityView = other.gameObject.GetComponent<CityView>();

      mainGameModel.mainMapMediators[view.GetLobbyCode()].SendMap(view.GetId(), cityView.GetId());
    }
    
    public static async Task WaitAsyncOperations(float sec)
    {
      await Task.Delay((int)(sec * 1000));
    }

    public override void OnRemove()
    {
    }
  }
}