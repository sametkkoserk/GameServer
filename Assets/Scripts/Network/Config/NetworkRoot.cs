using strange.extensions.context.impl;

namespace Network.Config
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
