using StrangeIoC.scripts.strange.extensions.context.impl;

namespace Runtime.Contexts.Network.Config
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
