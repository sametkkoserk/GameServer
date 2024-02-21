using System.Collections.Generic;
using System.Linq;
using Editor.Tools.DebugX.Runtime;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using Runtime.Modules.Core.ScreenManager.Vo;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
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
      dispatcher.AddListener(PanelEvent.CloseSpecificLayerPanels, OnCloseSpecificLayer);
      dispatcher.AddListener(PanelEvent.CloseSpecificPanel, OnCloseSpecificPanel);
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
        default:
          DebugX.Log(DebugKey.ScreenManager, "There is no Panel Mode like that!");
          return;
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
      instantiateAsync.Completed += handle =>
      {
        if (handle.Result == null)
          return;
        
        screenManagerModel.instantiatedPanels.Add(vo, instantiateAsync);
      };
      
      DebugX.Log(DebugKey.ScreenManager, $"{vo.addressableKey} Panel successfully created.");
    }

    #region Close Panel

    private void OnCloseLayerPanels(IEvent payload)
    {
      LayerKey layerKey = (LayerKey)payload.data;

      if (view.key != layerKey) return;
      
      DestroyAllChild();
    }

    private void OnCloseScenePanels(IEvent payload)
    {
      SceneKey sceneKey = (SceneKey)payload.data;

      if (gameObject.scene.name != sceneKey.ToString()) return;
      
      DestroyAllChild();
    }

    private void OnCloseAllPanels()
    {
      DestroyAllChild();
      
      screenManagerModel.instantiatedPanels.Clear();
    }

    private void OnCloseSpecificLayer(IEvent payload)
    {
      KeyValuePair<SceneKey, LayerKey> specificLayer = (KeyValuePair<SceneKey, LayerKey>)payload.data;

      if (gameObject.scene.name == specificLayer.Key.ToString() && view.key == specificLayer.Value)
      {
        DestroyAllChild();
      }
    }

    private void OnCloseSpecificPanel(IEvent payload)
    {
      string panelAddressableKey = (string)payload.data;

      for (int i = 0; i < screenManagerModel.instantiatedPanels.Count; i++)
      {
        if (screenManagerModel.instantiatedPanels.ElementAt(i).Key.addressableKey != panelAddressableKey) continue;
        
        Addressables.ReleaseInstance(screenManagerModel.instantiatedPanels.ElementAt(i).Value);
        screenManagerModel.instantiatedPanels.Remove(screenManagerModel.instantiatedPanels.ElementAt(i).Key);
        return;
      }
    }

    #endregion

    private void DestroyAllChild()
    {
      // Panel will have an closing animations. Destroy will change.

      for (int i = 0; i < transform.childCount; i++)
      {
        for (int j = 0; j < screenManagerModel.instantiatedPanels.Count; j++)
        {
          string key = screenManagerModel.instantiatedPanels.ElementAt(j).Key.addressableKey;
          
          if (transform.GetChild(i).name == key || transform.GetChild(i).name == key + "(Clone)")
            screenManagerModel.instantiatedPanels.Remove(screenManagerModel.instantiatedPanels.ElementAt(j).Key);
        }
        
        DestroyImmediate(transform.GetChild(i).gameObject);
      }
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(PanelContainerEvent.SetInitialData, SetInitialData);

      dispatcher.RemoveListener(PanelEvent.OpenPanel, OpenPanel);
      dispatcher.RemoveListener(PanelEvent.CloseAllPanels, OnCloseAllPanels);
      dispatcher.RemoveListener(PanelEvent.CloseScenePanels, OnCloseScenePanels);
      dispatcher.RemoveListener(PanelEvent.CloseLayerPanels, OnCloseLayerPanels);
      dispatcher.RemoveListener(PanelEvent.CloseSpecificLayerPanels, OnCloseSpecificLayer);
      dispatcher.RemoveListener(PanelEvent.CloseSpecificPanel, OnCloseSpecificPanel);
    }
  }
}