using strange.extensions.context.impl;

namespace Runtime.Modules.Core.GeneralContext
{
  public class GeneralRoot : ContextView
  {
    private void Awake()
    {
      context = new GeneralContext(this);
    }
  }
}