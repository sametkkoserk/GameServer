using Runtime.MainGame.Enum;
using Runtime.MainGame.Model.MainGameModel;
using Runtime.MainGame.Vo;
using strange.extensions.mediation.impl;

namespace Runtime.MainGame.View.MainMap
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