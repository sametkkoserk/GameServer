using System.Collections.Generic;
using System.Linq;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MiniGames.Enum;
using Runtime.Contexts.MiniGames.MiniGames;
using Runtime.Contexts.MiniGames.Model.MiniGamesModel;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Contexts.MiniGames.View.MiniGame
{
  public class MiniGameMediator : EventMediator
  {
    [Inject]
    public MiniGameView view { get; set; }

    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    [Inject]
    public IMiniGamesModel miniGamesModel { get; set; }

    public MiniGameController miniGameController;



    private void Start()
    {
      miniGamesModel.miniGameMediators[view.lobbyVo.lobbyCode] = this;
      SendPacketToLobbyVo<LobbyVo> vo = new SendPacketToLobbyVo<LobbyVo>()
      {
        clients = view.lobbyVo.clients,
        mainClass = view.lobbyVo
      };
      dispatcher.Dispatch(MiniGamesEvent.SendCreateMiniGameScene,vo);
      for (int i = 0; i < view.lobbyVo.clients.Count; i++)
      {
        view.lobbyVo.clients.ElementAt(i).Value.ready = false;
      }

      view.lobbyVo.readyCount = 0;
    }

    public void OnSceneReady(ushort clientId)
    {
      if (!view.lobbyVo.clients[clientId].ready)
      {
        view.lobbyVo.clients[clientId].ready = true;
        view.lobbyVo.readyCount++;
      }

      if (view.lobbyVo.readyCount!=view.lobbyVo.clients.Count)return;

      SendPacketToLobbyVo<MiniGameCreatedVo> sendPacketToLobbyVo = new()
      {
        mainClass = new MiniGameCreatedVo() { miniGameKey = view.miniGameKey },
        clients = view.lobbyVo.clients
      };
      dispatcher.Dispatch(MiniGamesEvent.MiniGameCreated, sendPacketToLobbyVo);

      Addressables.InstantiateAsync(view.miniGameKey, transform).Completed += handle =>
      {
        if (handle.Status != AsyncOperationStatus.Succeeded) return;
        handle.Result.transform.localPosition = Vector3.zero;
        miniGameController = handle.Result.GetComponent<MiniGameController>();
        miniGameController.miniGameMediator = this;
        miniGameController.lobbyVo = view.lobbyVo;
        miniGameController.Init();
      };
    }

    public void OnButtonClicked(ushort clientId, ClickedButtonsVo vo)
    {
      miniGameController.OnButtonClick(clientId, vo);
    }

    public void SendState(SendPacketToLobbyVo<MiniGameStateVo> stateVo)
    {
      dispatcher.Dispatch(MiniGamesEvent.SendState, stateVo);
    }

    public void EndTheGame(List<ushort> leaderBoard)
    {
      InfoVo vo = new() { clients = view.lobbyVo.clients };
      dispatcher.Dispatch(MiniGamesEvent.OnMiniGameEnded, vo);
      mainGameModel.MiniGameEnded(view.lobbyVo.lobbyCode, leaderBoard);
      miniGamesModel.MiniGameEnded(view.lobbyVo.lobbyCode);
      Destroy(gameObject);
    }

    public void SendMap(MiniGameMapGenerationVo vo)
    {
      dispatcher.Dispatch(MiniGamesEvent.SendMap,new SendPacketToLobbyVo<MiniGameMapGenerationVo>()
      {
        mainClass = vo,
        clients = view.lobbyVo.clients
      });
    }


  }
}