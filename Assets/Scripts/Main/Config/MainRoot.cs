using strange.extensions.context.impl;

namespace Main.Config
{
    public class MainRoot : ContextView
    {
        void Awake()
        {
            //Instantiate the context, passing it this instance.
            context = new MainContext(this);

        }
    }
}
