using strange.extensions.context.impl;

namespace MainGame.Config
{
    public class MainGameRoot : ContextView
    {
        void Awake()
        {
            //Instantiate the context, passing it this instance.
            context = new MainGameContext(this);

        }
    }
}
