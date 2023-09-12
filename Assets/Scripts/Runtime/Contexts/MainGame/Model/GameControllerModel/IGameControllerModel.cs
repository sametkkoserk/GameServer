using System;
using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.View.MainGameManager;
using Runtime.Contexts.MainGame.View.MainMap;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Vo;
using Runtime.Modules.Core.PromiseTool;

namespace Runtime.Contexts.MainGame.Model.GameControllerModel
{
    public interface IGameControllerModel
    {
      Dictionary<string, MainMapMediator> mainMapMediators { get; set; }
      Dictionary<string, MainGameManagerMediator> mainGameMediators { get; set; }

      void OnPlayerActionEnd(string lobbyCode);
      void OnGameStart(string lobbyCode, GameStartVo gameStartVo);
      void OnPlayerSceneReady(string LobbyCode, SceneReadyVo vo);
      void OnClaimCity(string cityVoLobbyCode, SendPacketWithLobbyCode<CityVo> cityVo);
    }
}