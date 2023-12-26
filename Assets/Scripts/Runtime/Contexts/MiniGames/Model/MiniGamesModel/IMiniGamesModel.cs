using System.Collections.Generic;
using Runtime.Contexts.MiniGames.View.MiniGame;
using Runtime.Contexts.MiniGames.Vo;

namespace Runtime.Contexts.MiniGames.Model.MiniGamesModel
{
    public interface IMiniGamesModel
    {
        List<string> miniGames { get; set; }
        Dictionary<string,MiniGameMediator> miniGameMediators { get; set; }

        string GetRandomMiniGame();
        void OnButtonClicked( ushort clientId, ClickedButtonsVo vo);
        void MiniGameEnded(string lobbyCode);
    }
}