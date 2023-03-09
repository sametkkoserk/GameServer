using System.Collections.Generic;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Vo;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel
{
  public interface IScreenManagerModel
  {
    Dictionary<string, int> layerMap { get; set; }
    
    List<string> sceneKeys { get; set; }
    
    Dictionary<PanelVo, AsyncOperationHandle<GameObject>> instantiatedPanels { get; set; }
    
    /// <summary>It adds new layer container to layerContainerDictionary to find it. uses <see cref="SceneKey"/>.</summary>
    /// <param name="sceneKey">Scene of the layer container.</param>
    void AddLayerContainer(string sceneKey);
    
    void SetSortOrder();
    
    /// <summary>Each panel has specific layer key. In the documentation you will see. TODO: We will share Documentation.</summary>
    /// <param name="panelAddressableKey">Addressable key of panel. Before the call you have to add addressable to prefab on inspector.</param>
    /// <param name="sceneKey">Scene of the layer container.</param>
    /// <param name="layerKey">Layer of the panel container.</param>
    /// <param name="panelMode">Method will operate according to panel mode.</param>
    /// <param name="panelType">Type of panel. We will add specific features to different panel types.</param>
    void OpenPanel(string panelAddressableKey, SceneKey sceneKey, LayerKey layerKey, PanelMode panelMode, PanelType panelType);

    /// <summary>Close all panels in the hierarchy.</summary>
    void CloseAllPanels();
    
    /// <summary>Close all panels in the specific scene.</summary>
    /// <param name="sceneKey">Key of the scene whose panels will be closed.</param>
    void CloseScenePanels(SceneKey sceneKey);
    
    /// <summary>Close all panels in the specific layer category.</summary>
    /// <param name="layerKey">Key of the layer whose panels will be closed.</param>
    void CloseLayerPanels(LayerKey layerKey);

    /// <summary>Close all panels in the specific layer.</summary>
    /// <param name="sceneKey">Key of the scene whose panels will be closed.</param>
    /// <param name="layerKey">Key of the layer whose panels will be closed.</param>
    void CloseSpecificLayer(SceneKey sceneKey, LayerKey layerKey);

    /// <summary>Close specific panel in the hierarchy.</summary>
    /// <param name="panelAddressableKey">Addressable key of panel.</param>
    void CloseSpecificPanel(string panelAddressableKey);
  }
}