using Runtime.Contexts.Lobby.Vo;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.MiniGames.View.MiniGame
{
    public class MiniGameView : EventView
    {
        public LobbyVo lobbyVo;
        
        public string miniGameKey;

        public void Init()
        {
            dispatcher.Dispatch(MiniGameEvent.Init);
        }
    }
}