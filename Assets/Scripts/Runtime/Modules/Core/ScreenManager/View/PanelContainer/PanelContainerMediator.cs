using Editor.Tools.DebugX.Runtime;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using Runtime.Modules.Core.ScreenManager.Vo;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Modules.Core.ScreenManager.View.PanelContainer
{
  public enum PanelContainerEvent
  {
    SetInitialData,
  }
  public class PanelContainerMediator : EventMediator
  {
    [Inject]
    public PanelContainerView view { get; set; }
    
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(PanelContainerEvent.SetInitialData, SetInitialData);
      
      dispatcher.AddListener(PanelEvent.OpenPanel, OpenPanel);
      dispatcher.AddListener(PanelEvent.CloseAllPanels, OnCloseAllPanels);
      dispatcher.AddListener(PanelEvent.CloseScenePanels, OnCloseScenePanels);
      dispatcher.AddListener(PanelEvent.CloseLayerPanels, OnCloseLayerPanels);
    }
    
    private void SetInitialData()
    {
      view.canvas.sortingOrder = screenManagerModel.layerMap[view.key.ToString()];
      
      gameObject.transform.name = view.key.ToString();
    }

    private void OpenPanel(IEvent payload)
    {
      PanelVo panelVo = (PanelVo)payload.data;

      if (panelVo.layerKey != view.key)
      {
        return;
      }

      switch (panelVo.panelMode)
      {
        case PanelMode.Destroy:
          OnDestroyPanelContainer(panelVo);
          break;
        case PanelMode.Additive:
          OnAdditivePanelContainer(panelVo);
          break;
        case PanelMode.HideOthers:
          OnHidePanelContainer(panelVo);
          break;
      }
    }

    private void OnDestroyPanelContainer(PanelVo panelVo)
    {
      DestroyAllChild();
      
      CreatePanel(panelVo);
    }

    private void OnAdditivePanelContainer(PanelVo panelVo)
    {
      CreatePanel(panelVo);
    }
    
    private void OnHidePanelContainer(PanelVo panelVo)
    {
      for (int i = 0; i < gameObject.transform.childCount; i++)
      {
        transform.GetChild(i).gameObject.SetActive(false);
      }
      
      CreatePanel(panelVo);
    }

    private void CreatePanel(PanelVo vo)
    {
      AsyncOperationHandle<GameObject> instantiateAsync = Addressables.InstantiateAsync(vo.addressableKey, transform);
      instantiateAsync.Completed += handle => { view.CurrentPanel = instantiateAsync.Result; };
      DebugX.Log(DebugKey.ScreenManager, "Panel successfully created.");
    }

    #region Close Panel
    private void OnCloseLayerPanels(IEvent payload)
    {
      LayerKey layerKey = (LayerKey)payload.data;

      if (view.key == layerKey)
      {
        DestroyAllChild();
      }
    }

    private void OnCloseScenePanels(IEvent payload)
    {
      SceneKey sceneKey = (SceneKey)payload.data;

      if (gameObject.scene.name == sceneKey.ToString())
      {
        DestroyAllChild();
      }
    }

    private void OnCloseAllPanels()
    {
      DestroyAllChild();
    }
    
    #endregion

    private void DestroyAllChild()
    {
      // Panel will have an closing animations. Destroy will change.

      for (int i = 0; i < gameObject.transform.childCount; i++)
      {
        DestroyImmediate(transform.GetChild(i).gameObject);
      }
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(PanelContainerEvent.SetInitialData, SetInitialData);
      
      dispatcher.RemoveListener(PanelEvent.OpenPanel, OpenPanel);
    }
  }
}