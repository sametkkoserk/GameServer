using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;
using Random = System.Random;

namespace Runtime.Contexts.MainGame.View.MainGameManager
{
  public class MainGameManagerMediator : EventMediator
  {
    [Inject]
    public MainGameManagerView view { get; set; }

    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void OnRegister()
    {
    }

    /// <summary>It checks everyone see the main map and ready to start. If each player ready, method will determine queue of players.</summary>
    private void Start()
    {
      Init();
    }

    private async Task Init()
    {
      view.lobbyVo = mainGameModel.managerLobbyVos[0];
      mainGameModel.managerLobbyVos.Remove(view.lobbyVo);
      mainGameModel.mainGameMediators[view.lobbyVo.lobbyCode] = this;

      for (int i = 0; i < view.lobbyVo.clients.Count; i++)
      {
        view.gameManagerVo.playerFeaturesVos.Add(view.lobbyVo.clients.ElementAt(i).Key, new PlayerFeaturesVo());
      }

      SetRandomQueue();

      view.gameManagerVo.queue = -1;

      ChangeGameState(GameStateKey.ClaimCity);

      List<PlayerActionKey> playerActionKey = new() { PlayerActionKey.OpenDetailsPanel };
      for (int i = 0; i < view.lobbyVo.clients.Count; i++)
        view.gameManagerVo.playerActionVo.playerActionKeys.Add(view.lobbyVo.clients.ElementAt(i).Value.id, playerActionKey);
      dispatcher.Dispatch(MainGameEvent.ChangePlayerAction, view.gameManagerVo.playerActionVo);

      await NextTurn();
    }

    #region Turn

    private async Task NextTurn()
    {
      StopAllCoroutines();

      await WaitAsyncOperations(PanelClosingTimes.breath);

      // In the future connection information must be checked. If player is AFK, it will be the next player's turn.
      RemovePreviousTurnsActions();

      view.gameManagerVo.queue++;
      if (view.gameManagerVo.queue >= view.gameManagerVo.queueList.Count)
        view.gameManagerVo.queue = 0;

      view.gameManagerVo.turnVo.id = view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue);
      view.gameManagerVo.turnVo.remainingTime = view.lobbyVo.lobbySettingsVo.turnTime;

      await CheckingSystemsAfterTurn();

      if (!view.gameManagerVo.startTimer)
        return;

      SendPacketToLobbyVo<TurnVo> vo = networkManager.SetSendPacketToLobbyVo(view.gameManagerVo.turnVo, view.lobbyVo.clients);
      dispatcher.Dispatch(MainGameEvent.NextTurn, vo);

      await WaitAsyncOperations(PanelClosingTimes.nextTurn);
      StartCoroutine(SetTimer());
    }

    public async Task ChangeTurn()
    {
      await NextTurn();
    }

    private IEnumerator SetTimer()
    {
      yield return new WaitForSeconds(1f);

      if (view.gameManagerVo.turnVo.remainingTime <= 0f)
      {
        AfterTurnTimeOver();
        yield break;
      }

      view.gameManagerVo.turnVo.remainingTime--;

      SendPacketToLobbyVo<TurnVo> vo = networkManager.SetSendPacketToLobbyVo(view.gameManagerVo.turnVo, view.lobbyVo.clients);
      dispatcher.Dispatch(MainGameEvent.SendRemainingTime, vo);

      StartCoroutine(SetTimer());
    }

    private void RemovePreviousTurnsActions()
    {
      if (view.gameManagerVo.queue <= 0)
        return;
      
      ushort id = view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue);
      List<ushort> ids = new() { id };
      
      switch (view.gameManagerVo.gameStateVo.gameStateKey)
      {
        case GameStateKey.ClaimCity:
          UpdatePlayerActionKeys(ids, PlayerActionKey.ClaimCity, false);
          break;
        case GameStateKey.Arming:
          UpdatePlayerActionKeys(ids, PlayerActionKey.Arming, false);
          break;
        default:
          return;
      }
    }

    private async Task CheckingSystemsAfterTurn()
    {
      GameStateKey gameStateKey = view.gameManagerVo.gameStateVo.gameStateKey;
      
      switch (gameStateKey)
      {
        case GameStateKey.ClaimCity:
          await ClaimCitySystems();
          break;
        case GameStateKey.MiniGame:
          await MiniGameSystems();
          break;
        case GameStateKey.Arming:
          await ArmingSystems();
          break;
        default:
          view.gameManagerVo.startTimer = false;
          break;
      }
    }

    private async Task ClaimCitySystems()
    {
      if (mainGameModel.mainMapMediators[view.lobbyVo.lobbyCode].GetEmptyCities().Count == 0)
      {
        ChangeGameState(GameStateKey.MiniGame);
        view.gameManagerVo.startTimer = false;
        await CheckingSystemsAfterTurn();
      }
      else
      {
        ushort nextId = view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue);
        List<ushort> ids = new() { nextId };
        UpdatePlayerActionKeys(ids, PlayerActionKey.ClaimCity, true);
        view.gameManagerVo.startTimer = true;
      }
    }

    private async Task MiniGameSystems()
    {
      SetRandomQueue();
      SetRewards();
      
      view.gameManagerVo.queueList.Reverse();
      view.gameManagerVo.queue = -1;
      view.gameManagerVo.startTimer = false;
      
      ChangeGameState(GameStateKey.Arming);

      await WaitAsyncOperations(PanelClosingTimes.miniGameResults);
      
      ClearAllPlayerActions();
      UpdateAllPlayerActions(new List<PlayerActionKey> { PlayerActionKey.OpenDetailsPanel }, true);
      
      StartCoroutine(Breath());
    }

    private IEnumerator Breath()
    {
      yield return new WaitForSeconds(0.5f);
      NextTurn();
    }

    private void SetRewards()
    {
      List<MiniGameStatsVo> miniGameStatsVos = new();

      for (int i = 0; i < view.gameManagerVo.queueList.Count; i++)
      {
        ushort id = view.gameManagerVo.queueList.ElementAt(i);
        int coefficient = view.gameManagerVo.queueList.Count - i;

        int newSoldiers = coefficient * 2;
        view.gameManagerVo.playerFeaturesVos[id].freeSoldierCount += newSoldiers;

        List<string> rewards = new()
        {
          (ushort)(i + 1) + ". Place Bonus",
          newSoldiers + " Soldiers"
        };

        MiniGameStatsVo miniGameStatsVo = new()
        {
          playerArrangement = (ushort)(i + 1),
          playerRewards = rewards,
          playerId = id,
          playerFeaturesVo = view.gameManagerVo.playerFeaturesVos[id]
        };

        miniGameStatsVos.Add(miniGameStatsVo);
      }

      miniGameStatsVos.ElementAt(0).clients = view.lobbyVo.clients;
      dispatcher.Dispatch(MainGameEvent.MiniGameRewards, miniGameStatsVos);
    }

    private async Task ArmingSystems()
    {
      ushort nextId = view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue);
      List<ushort> ids = new() { nextId };
      UpdatePlayerActionKeys(ids, PlayerActionKey.Arming, true);
      view.gameManagerVo.startTimer = true;
    }

    public void ArmingToCity(ArmingVo armingVo)
    {
      view.gameManagerVo.playerFeaturesVos[armingVo.clientId].freeSoldierCount -= armingVo.soldierCount;
      
      PlayerFeaturesVo playerFeaturesVo = view.gameManagerVo.playerFeaturesVos[armingVo.clientId];
      playerFeaturesVo.clientId = armingVo.clientId;
      
      dispatcher.Dispatch(MainGameEvent.ArmingCity, playerFeaturesVo);
    }

    private async Task AfterTurnTimeOver()
    {
      if (view.gameManagerVo.gameStateVo.gameStateKey == GameStateKey.ClaimCity)
      {
        ushort queueId = view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue);
        mainGameModel.mainMapMediators[view.lobbyVo.lobbyCode].AssignCityRandomly(queueId);
      }

      await NextTurn();
    }

    #endregion

    #region PlayerActions

    private void UpdatePlayerActionKeys(List<ushort> idList, List<PlayerActionKey> playerActionKeys, bool add)
    {
      UpdatePlayerActionsMain(idList, playerActionKeys, add);
    }

    private void UpdatePlayerActionKeys(List<ushort> idList, PlayerActionKey playerActionKey, bool add)
    {
      List<PlayerActionKey> newList = new() { playerActionKey };

      UpdatePlayerActionsMain(idList, newList, add);
    }

    private void UpdateAllPlayerActions(List<PlayerActionKey> playerActionKeys, bool add)
    {
      List<ushort> idList = new();

      for (int i = 0; i < view.gameManagerVo.playerActionVo.playerActionKeys.Count; i++)
      {
        KeyValuePair<ushort, List<PlayerActionKey>> userActionKeys = view.gameManagerVo.playerActionVo.playerActionKeys.ElementAt(i);
        idList.Add(userActionKeys.Key);
      }
      
      UpdatePlayerActionsMain(idList, playerActionKeys, add);
    }

    private void ClearAllPlayerActions()
    {
      for (int i = 0; i < view.gameManagerVo.playerActionVo.playerActionKeys.Count; i++)
      {
        ushort id = view.gameManagerVo.playerActionVo.playerActionKeys.ElementAt(i).Key;
        view.gameManagerVo.playerActionVo.playerActionKeys[id] = new List<PlayerActionKey> {};
      }
    }

    private void UpdatePlayerActionsMain(List<ushort> idList, List<PlayerActionKey> playerActionKeyList, bool add)
    {
      PlayerActionVo vo = new();

      for (int i = 0; i < idList.Count; i++)
      {
        List<PlayerActionKey> playerActionKeys = view.gameManagerVo.playerActionVo.playerActionKeys[idList[i]];

        if (add)
        {
          for (int j = 0; j < playerActionKeyList.Count; j++)
          {
            if (playerActionKeys.Contains(playerActionKeyList[j]))
              continue;

            playerActionKeys.Add(playerActionKeyList[j]);
          }
        }
        else
        {
          for (int j = 0; j < playerActionKeyList.Count; j++)
          {
            if (playerActionKeys.Contains(playerActionKeyList[j]))
              playerActionKeys.Remove(playerActionKeyList[j]);
          }
        }

        vo.playerActionKeys.Add(idList[i], playerActionKeys);
      }

      dispatcher.Dispatch(MainGameEvent.ChangePlayerAction, vo);
    }

    private void ChangeGameState(GameStateKey gameStateKey)
    {
      view.gameManagerVo.gameStateVo.gameStateKey = gameStateKey;
      SendPacketToLobbyVo<GameStateVo> gameStateVo = networkManager.SetSendPacketToLobbyVo(view.gameManagerVo.gameStateVo, view.lobbyVo.clients);
      dispatcher.Dispatch(MainGameEvent.ChangeGameState, gameStateVo);
    }

    #endregion

    private void SetRandomQueue()
    {
      Random random = new();

      List<ushort> randomKeys = view.lobbyVo.clients.Keys.ToList();

      view.gameManagerVo.queueList = randomKeys.OrderBy(_ => random.Next()).ToList();
    }

    public static async Task WaitAsyncOperations(float sec)
    {
      await Task.Delay((int)(sec * 1000));
    }

    public override void OnRemove()
    {
    }
  }
}