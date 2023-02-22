using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.Vo;
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