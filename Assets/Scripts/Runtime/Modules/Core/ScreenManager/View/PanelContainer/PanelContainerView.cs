using Runtime.Modules.Core.ScreenManager.Enum;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Modules.Core.ScreenManager.View.PanelContainer
{
  public class PanelContainerView : EventView
  {
    public Canvas canvas;
    
    public LayerKey key;
    
    [HideInInspector]
    public GameObject CurrentPanel;

    public void Init(LayerKey _layerKey)
    {
      key = _layerKey;
      
      dispatcher.Dispatch(PanelContainerEvent.SetInitialData);
    }
  }
}