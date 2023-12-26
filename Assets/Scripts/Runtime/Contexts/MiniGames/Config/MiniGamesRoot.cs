using strange.extensions.context.impl;

namespace Runtime.Contexts.MiniGames.Config
{
    public class MiniGamesRoot : ContextView
    {
        void Awake()
        {
            //Instantiate the context, passing it this instance.
            context = new MiniGamesContext(this);

        }
    }
}
