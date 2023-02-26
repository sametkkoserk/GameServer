using System.Collections.Generic;
using System.Linq;
using Editor.Tools.DebugX.Runtime;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Vo;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel
{
  public class ScreenManagerModel : IScreenManagerModel
  {
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher{ get; set;}
    public Dictionary<string, int> layerMap { get; set; }
    public List<string> sceneKeys { get; set; }
    
    [PostConstruct]
    public void OnPostConstruct()
    {
      layerMap = new Dictionary<string, int>();
      sceneKeys = new List<string>();
    }

    public void AddLayerContainer(string sceneKey)
    {
      sceneKeys.Add(sceneKey);
      
      DebugX.Log(DebugKey.ScreenManager, $"New Scene added to Layer Container: {sceneKey}");
    }

    public void SetSortOrder()
    {
      string[] layers = System.Enum.GetNames(typeof(LayerKey));
      layers = layers.Reverse().ToArray();
      for (int i = 0; i < layers.Length; i++)
      {
        layerMap.Add(layers[i], i * 10);
      }
    }
    
    public void OpenPanel(SceneKey sceneKey, LayerKey layerKey, PanelMode panelMode, PanelType panelType, string panelAddressableKey)
    {
      if (!sceneKeys.Contains(sceneKey.ToString()))
      {
        // If panel does not stop in there and nothing happens, there is a sequence problem or scene does not exist in the hierarchy.
        DebugX.Log(DebugKey.ScreenManager, "There is no Scene key like that!");
        return;
      }
      
      if (!layerMap.ContainsKey(layerKey.ToString()))
      {
        DebugX.Log(DebugKey.ScreenManager, "There is no Layer key like that!");
        return;
      }
      
      PanelVo vo = new()
      {
        layerKey = layerKey,
        panelMode = panelMode,
        panelType = panelType,
        addressableKey = panelAddressableKey
      };
      
      dispatcher.Dispatch(PanelEvent.OpenPanel, vo);
    }

    public void CloseAllPanels()
    {
      dispatcher.Dispatch(PanelEvent.CloseAllPanels);
    }

    public void CloseScenePanels(SceneKey sceneKey)
    {
      dispatcher.Dispatch(PanelEvent.CloseScenePanels, sceneKey);
    }

    public void CloseLayerPanels(LayerKey layerKey)
    {
      dispatcher.Dispatch(PanelEvent.CloseLayerPanels, layerKey);
    }
  }
}