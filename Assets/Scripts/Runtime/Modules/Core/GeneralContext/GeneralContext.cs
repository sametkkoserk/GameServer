using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using Runtime.Modules.Core.ScreenManager.View.LayerContainer;
using Runtime.Modules.Core.ScreenManager.View.PanelContainer;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Runtime.Modules.Core.GeneralContext
{
  public class GeneralContext : MVCSContext
  {
    public GeneralContext(MonoBehaviour view) : base(view)
    {
    }

    public GeneralContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      base.mapBindings();

      injectionBinder.Bind<IScreenManagerModel>().To<ScreenManagerModel>().ToSingleton();

      mediationBinder.Bind<LayerContainerView>().To<LayerContainerMediator>();
      mediationBinder.Bind<PanelContainerView>().To<PanelContainerMediator>();
    }
  }
}