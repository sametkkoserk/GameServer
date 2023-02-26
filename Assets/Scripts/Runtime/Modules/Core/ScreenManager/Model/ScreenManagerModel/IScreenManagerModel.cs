using System.Collections.Generic;
using Runtime.Modules.Core.ScreenManager.Enum;

namespace Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel
{
  public interface IScreenManagerModel
  {
    Dictionary<string, int> layerMap { get; set; }
    
    List<string> sceneKeys { get; set; }
    
    /// <summary>It adds new layer container to layerContainerDictionary to find it. uses <see cref="SceneKey"/>.</summary>
    /// <param name="sceneKey">Scene of the layer container.</param>
    void AddLayerContainer(string sceneKey);
    
    void SetSortOrder();
    
    /// <summary>Each panel has specific layer key. In the documentation you will see. TODO: We will share Documentation.</summary>
    /// <param name="sceneKey">Scene of the layer container.</param>
    /// <param name="layerKey">Layer of the panel container.</param>
    /// <param name="panelMode">Method will operate according to panel mode.</param>
    /// <param name="panelType">Type of panel. We will add specific features to different panel types.</param>
    /// <param name="panelAddressableKey">Addressable key of panel. Before the call you have to add addressable to prefab on inspector.</param>
    void OpenPanel(SceneKey sceneKey, LayerKey layerKey, PanelMode panelMode, PanelType panelType, string panelAddressableKey);

    /// <summary>Close all panels in the scenes.</summary>
    void CloseAllPanels();
    
    /// <summary>Close all panels in the specific scenes.</summary>
    /// <param name="sceneKey">Key of the scene whose panels will be closed.</param>
    void CloseScenePanels(SceneKey sceneKey);
    
    /// <summary>Close all panels in the specific scenes.</summary>
    /// <param name="layerKey">Key of the layer whose panels will be closed.</param>
    void CloseLayerPanels(LayerKey layerKey);
  }
}