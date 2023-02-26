using System.Collections.Generic;
using Runtime.Modules.Core.ScreenManager.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Modules.Core.ScreenManager.View.LayerContainer
{
  public class LayerContainerView : EventView
  {
    public GameObject panelContainer;

    public List<LayerVo> layerVos;

    [HideInInspector]
    public List<string> processedKeys;

  }
}