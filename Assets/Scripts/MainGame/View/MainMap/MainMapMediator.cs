using strange.extensions.mediation.impl;

namespace MainGame.View.MainMap
{
  public enum MainMapEvent
  {
    
  }
  public class MainMapMediator : EventMediator
  {
    [Inject]
    public MainMapView view { get; set; }

    public override void OnRegister()
    {
    }

    public override void OnRemove()
    {
    }
  }
}