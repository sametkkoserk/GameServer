using System;
using System.Collections.Generic;
using Runtime.Contexts.MiniGames.View.MiniGame;
using Runtime.Contexts.MiniGames.Vo;
using UnityEngine;

namespace Runtime.Contexts.MiniGames.Model.MiniGamesModel
{
    public interface IMiniGamesModel
    {
        List<string> miniGames { get; set; }
        Dictionary<string,MiniGameMediator> miniGameMediators { get; set; }

        string GetRandomMiniGame();
        void OnButtonClicked( ushort clientId, ClickedButtonsVo vo);
        void MiniGameEnded(string lobbyCode);
        void OnMiniGameSceneReady(string voLobbyCode, ushort clientId);
        void CreateNewGame(string key, Transform parent = null, Action<GameObject> action = null);

    }
}