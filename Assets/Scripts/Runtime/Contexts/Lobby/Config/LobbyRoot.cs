using strange.extensions.context.impl;

namespace Runtime.Contexts.Lobby.Config
{
    public class LobbyRoot : ContextView
    {
        void Awake()
        {
            //Instantiate the context, passing it this instance.
            context = new LobbyContext(this);

        }
    }
}
