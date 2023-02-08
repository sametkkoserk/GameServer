using strange.extensions.context.impl;

namespace Multiplayer.Config
{
    public class NetworkRoot : ContextView
    {
        void Awake()
        {
            //Instantiate the context, passing it this instance.
            context = new NetworkContext(this);

        }
    }
}
