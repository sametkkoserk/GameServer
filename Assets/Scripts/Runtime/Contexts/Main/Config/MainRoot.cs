using StrangeIoC.scripts.strange.extensions.context.impl;

namespace Runtime.Contexts.Main.Config
{
    public class MainRoot : ContextView
    {
        private void Awake()
        {
            context = new MainContext(this);
        }
    }
}
