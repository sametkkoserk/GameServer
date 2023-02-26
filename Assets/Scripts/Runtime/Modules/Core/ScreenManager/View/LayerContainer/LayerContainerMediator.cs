using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using Runtime.Modules.Core.ScreenManager.View.PanelContainer;
using Runtime.Modules.Core.ScreenManager.Vo;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Modules.Core.ScreenManager.View.LayerContainer
{
  public class LayerContainerMediator : EventMediator
  {
    [Inject]
    public LayerContainerView view { get; set; }
    
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void OnRegister()
    {
      Init();
    }

    private void Init()
    {
      screenManagerModel.AddLayerContainer(transform.gameObject.scene.name);
    }

    private void Start()
    {
      screenManagerModel.SetSortOrder();
      
      for (int i = 0; i < view.layerVos.Count; i++)
      {
        if (!view.layerVos[i].active) continue;
        if (view.processedKeys.Contains(view.layerVos[i].key.ToString())) continue;

        view.processedKeys.Add(view.layerVos[i].key.ToString());
        
        GameObject instantiated = Instantiate(view.panelContainer, transform);
        PanelContainerView behaviour = instantiated.GetComponent<PanelContainerView>();

        behaviour.Init(view.layerVos[i].key);
      }
      
      //TODO: Şafak: burada dispatch at ve ppanellerin açılmasını söyleyen yerler ona göre dinlesin. Her context için geçerli.
      dispatcher.Dispatch(PanelEvent.PanelContainersCreated);
    }

    public override void OnRemove()
    {
    }
  }
}