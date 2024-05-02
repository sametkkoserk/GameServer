using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.MiniGames.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;
using Random = System.Random;

namespace Runtime.Contexts.MainGame.View.MainGameManager
{
  public class MainGameManagerMediator : EventMediator
  {
    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }
    
    [Inject]
    public MainGameManagerView view { get; set; }

    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void OnRegister()
    {
    }

    private void Start()
    {
      Init();
    }

    private void Init()
    {
      view.lobbyVo = mainGameModel.managerLobbyVos[0];
      mainGameModel.managerLobbyVos.Remove(view.lobbyVo);
      mainGameModel.mainGameManagerMediators[view.lobbyVo.lobbyCode] = this;
      view.mainMapMediator = mainGameModel.mainMapMediators[view.lobbyVo.lobbyCode];
      view.mainMapMediator.SetMainGameManager();

      for (int i = 0; i < view.lobbyVo.clients.Count; i++)
      {
        view.gameManagerVo.playerFeaturesVos.Add(view.lobbyVo.clients.ElementAt(i).Key,
          new PlayerFeaturesVo
          {
            freeSoldierCount = mainGameModel.ClaimCitySoldierCount,
            clientId = view.lobbyVo.clients.ElementAt(i).Key
          });
      }

      view.gameManagerVo.claimCitySoldierCount = mainGameModel.ClaimCitySoldierCount;

      OnStartMiniGame();
    }

    private async Task NextTurn()
    {
      if (!view.gameManagerVo.nextTurn)
        return;

      StopAllCoroutines();

      await WaitAsyncOperations(PanelClosingTimes.breath);

      view.gameManagerVo.queue++;
      if (view.gameManagerVo.queue >= view.gameManagerVo.queueList.Count)
        view.gameManagerVo.queue = 0;

      view.gameManagerVo.turnVo.id = view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue);
      view.gameManagerVo.turnVo.remainingTime = view.lobbyVo.lobbySettingsVo.turnTime;

      CheckingSystemsAfterTurn();

      if (!view.gameManagerVo.startTimer)
        return;

      SendPacketToLobbyVo<TurnVo> vo = networkManager.SetSendPacketToLobbyVo(view.gameManagerVo.turnVo, view.lobbyVo.clients);
      dispatcher.Dispatch(MainGameEvent.NextTurn, vo);

      await WaitAsyncOperations(PanelClosingTimes.nextTurn);

      StartCoroutine(SetTimer());
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

    private void AfterTurnTimeOver()
    {
      if (view.gameManagerVo.gameStateVo.gameStateKey == GameStateKey.ClaimCity)
      {
        ushort queueId = view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue);
        view.mainMapMediator.AssignCityRandomly(queueId);
        NextTurn();
      }
      else
      {
        if (CheckLastPlayer())
        {
          ChangeGameState(GameStateKey.MiniGame);

          view.gameManagerVo.startTimer = false;
          view.gameManagerVo.nextTurn = false;

          OnStartMiniGame();
          return;
        }

        ChangeGameState(GameStateKey.Arming);
        NextTurn();
      }
      // else if (view.gameManagerVo.gameStateVo.gameStateKey == GameStateKey.Attack)
      // {
      //   if (view.gameManagerVo.uncompletedAttackCityVos.Count != 0)
      //   {
      //     KeyValuePair<int, int> element = view.gameManagerVo.uncompletedAttackCityVos.ElementAt(0);
      //     FortifyVo fortifyVo = new()
      //     {
      //       sourceCityId = element.Key,
      //       targetCityId = element.Value,
      //       soldierCount = 1,
      //       clientId = view.gameManagerVo.clientId
      //     };
      //   
      //     // OnFortify(fortifyVo);
      //   }
      // }
    }

    private void CheckingSystemsAfterTurn()
    {
      GameStateKey gameStateKey = view.gameManagerVo.gameStateVo.gameStateKey;

      switch (gameStateKey)
      {
        case GameStateKey.ClaimCity:
          ClaimCitySystem();
          break;
        case GameStateKey.MiniGame:
          OnStartMiniGame();
          break;
        // case GameStateKey.Arming:
        //   await ArmingSystem();
        //   break;
        // case GameStateKey.Attack:
        //   await AttackSystem();
        //   break;
        // case GameStateKey.Fortify:
        //   await FortifySystem();
        //   break;
      }
    }

    public void OnPass(ushort clientId)
    {
      if (clientId != view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue))
        return;

      if (view.gameManagerVo.gameStateVo.gameStateKey == GameStateKey.ClaimCity)
      {
        ushort queueId = view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue);
        view.mainMapMediator.AssignCityRandomly(queueId);
        NextTurn();
      }
      else if (view.gameManagerVo.gameStateVo.gameStateKey == GameStateKey.Arming)
      {
        ChangeGameState(GameStateKey.Attack);
      }
      else if (view.gameManagerVo.gameStateVo.gameStateKey == GameStateKey.Attack)
      {
        view.gameManagerVo.uncompletedAttackCityVos.Clear();
        
        ChangeGameState(GameStateKey.Fortify);
      }
      else if (view.gameManagerVo.gameStateVo.gameStateKey == GameStateKey.Fortify)
      {
        if (CheckLastPlayer())
        {
          ChangeGameState(GameStateKey.MiniGame);

          view.gameManagerVo.startTimer = false;
          view.gameManagerVo.nextTurn = false;

          OnStartMiniGame();
        }
        else
        {
          ChangeGameState(GameStateKey.Arming);
          NextTurn();
        }
      }
    }

    public void ChangeTurn()
    {
      NextTurn();
    }

    #region MiniGame

    private void OnStartMiniGame()
    {
      view.gameManagerVo.startTimer = false;
      // SetRandomQueue();
      // OnMiniGameEnded(view.gameManagerVo.queueList);
      crossDispatcher.Dispatch(MiniGamesEvent.OnCreateMiniGame,view.lobbyVo);
      
    }

    public async void OnMiniGameEnded(List<ushort> tourQueue)
    {
      view.gameManagerVo.queueList = tourQueue;
      view.gameManagerVo.queue = -1;
      
      SetRewards();

      await WaitAsyncOperations(PanelClosingTimes.miniGameResults);

      view.gameManagerVo.startTimer = true;
      view.gameManagerVo.nextTurn = true;

      if (!view.gameManagerVo.claimCityEnded)
      {
        await NextTurn();
        return;
      }

      ChangeGameState(GameStateKey.Arming);
      await NextTurn();
    }

    private void SetRandomQueue()
    {
      Random random = new();

      List<ushort> randomKeys = view.lobbyVo.clients.Keys.ToList();

      view.gameManagerVo.queueList = randomKeys.OrderBy(_ => random.Next()).ToList();
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

    #endregion

    #region ClaimCity

    private void ClaimCitySystem()
    {
      if (view.gameManagerVo.playerFeaturesVos[GetLastPlayer()].clientId == GetCurrentPlayer())
      {
        view.gameManagerVo.claimCitySoldierCount--;

        if (view.gameManagerVo.claimCitySoldierCount >= 0) return;
      }
      else
      {
        return;
      }

      view.gameManagerVo.claimCityEnded = true;
      ChangeGameState(GameStateKey.MiniGame);
      CheckingSystemsAfterTurn();
    }

    #endregion

    #region Arming

    public void ArmingToCity(ArmingVo armingVo)
    {
      view.gameManagerVo.playerFeaturesVos[armingVo.clientId].freeSoldierCount -= armingVo.soldierCount;

      PlayerFeaturesVo playerFeaturesVo = view.gameManagerVo.playerFeaturesVos[armingVo.clientId];
      playerFeaturesVo.clientId = armingVo.clientId;

      dispatcher.Dispatch(MainGameEvent.ChangePlayerFeature, playerFeaturesVo);
    }

    #endregion

    #region Attack

    public void OnAttack(AttackVo attackVo)
    {
      if (view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue) != attackVo.clientId)
        return;

      CityVo attackerCityVo = view.mainMapMediator.view.cities[attackVo.attackerCityID];
      CityVo defenderCityVo = view.mainMapMediator.view.cities[attackVo.defenderCityID];

      if (attackerCityVo.ownerID != attackVo.clientId)
        return;

      if (attackerCityVo.soldierCount > defenderCityVo.soldierCount)
      {
        AttackerWin(attackerCityVo, defenderCityVo);
      }
      else if (attackerCityVo.soldierCount <= defenderCityVo.soldierCount)
      {
        DefenderWin(attackerCityVo, defenderCityVo);
      }
      else
      {
        return;
      }

      view.gameManagerVo.uncompletedAttackCityVos.Clear();
      view.gameManagerVo.uncompletedAttackCityVos.Add(attackerCityVo.ID, defenderCityVo.ID);
      view.gameManagerVo.clientId = attackerCityVo.clientId;
    }

    private void AttackerWin(CityVo attacker, CityVo defender)
    {
      CityVo attackerCity = view.mainMapMediator.view.GetSpecificCity(attacker.ID);
      CityVo defenderCity = view.mainMapMediator.view.GetSpecificCity(defender.ID);

      attackerCity.soldierCount -= defenderCity.soldierCount;
      if (attackerCity.soldierCount == 0)
        attackerCity.soldierCount = 1;
      view.mainMapMediator.view.SetSpecificCity(attackerCity);

      defenderCity.soldierCount = 1;
      defenderCity.ownerID = attackerCity.ownerID;
      view.mainMapMediator.view.SetSpecificCity(defenderCity);

      AttackResultVo resultVo = new()
      {
        winnerCity = attackerCity,
        loserCity = defenderCity,
        isConquered = true
      };
      SendPacketToLobbyVo<AttackResultVo> vo = networkManager.SetSendPacketToLobbyVo(resultVo, view.lobbyVo.clients);

      dispatcher.Dispatch(MainGameEvent.AttackResult, vo);
    }

    private void DefenderWin(CityVo attacker, CityVo defender)
    {
      CityVo attackerCity = view.mainMapMediator.view.GetSpecificCity(attacker.ID);
      CityVo defenderCity = view.mainMapMediator.view.GetSpecificCity(defender.ID);

      defenderCity.soldierCount -= attackerCity.soldierCount;
      if (defenderCity.soldierCount == 0)
        defenderCity.soldierCount = 1;
      view.mainMapMediator.view.SetSpecificCity(defenderCity);

      attackerCity.soldierCount = 1;
      view.mainMapMediator.view.SetSpecificCity(attackerCity);

      AttackResultVo resultVo = new()
      {
        winnerCity = defenderCity,
        loserCity = attackerCity,
        isConquered = false
      };

      SendPacketToLobbyVo<AttackResultVo> vo = networkManager.SetSendPacketToLobbyVo(resultVo, view.lobbyVo.clients);

      dispatcher.Dispatch(MainGameEvent.AttackResult, vo);
    }

    #endregion

    #region Fortify

    public void OnFortify(FortifyVo fortifyVo)
    {
      if (view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue) != fortifyVo.clientId)
        return;

      CityVo sourceCityVo = view.mainMapMediator.view.cities[fortifyVo.sourceCityId];
      CityVo targetCityVo = view.mainMapMediator.view.cities[fortifyVo.targetCityId];

      if (sourceCityVo.ownerID != fortifyVo.clientId)
        return;

      if (targetCityVo.ownerID != fortifyVo.clientId)
        return;

      if (view.gameManagerVo.uncompletedAttackCityVos.Count != 0)
      {
        KeyValuePair<int, int> element = view.gameManagerVo.uncompletedAttackCityVos.ElementAt(0);
        if (fortifyVo.sourceCityId != element.Key || fortifyVo.targetCityId != element.Value)
          return;

        view.gameManagerVo.uncompletedAttackCityVos.Clear();
      }

      if (sourceCityVo.soldierCount - fortifyVo.soldierCount <= 0)
        return;

      targetCityVo.soldierCount += fortifyVo.soldierCount;
      sourceCityVo.soldierCount -= fortifyVo.soldierCount;

      FortifyResultVo resultVo = new()
      {
        targetCity = targetCityVo,
        sourceCity = sourceCityVo
      };
      SendPacketToLobbyVo<FortifyResultVo> vo = networkManager.SetSendPacketToLobbyVo(resultVo, view.lobbyVo.clients);

      dispatcher.Dispatch(MainGameEvent.FortifyResult, vo);
    }

    #endregion

    public int GetCurrentPlayer()
    {
      return view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue);
    }

    public bool CheckLastPlayer()
    {
      ushort queueId = view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue);

      return queueId == view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queueList.Count - 1);
    }

    public ushort GetLastPlayer()
    {
      return view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queueList.Count - 1);
    }

    public bool IsClientTurn(ushort clientId)
    {
      ushort queueId = view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue);
      return queueId == clientId;
    }

    public bool IsCityNeutral(int cityId)
    {
      ushort ownerId = view.mainMapMediator.view.cities[cityId].ownerID;
      return ownerId == 0;
    }

    public bool IsCityOwnerTheClient(int cityId, ushort clientId)
    {
      ushort ownerId = view.mainMapMediator.view.cities[cityId].ownerID;
      return ownerId == clientId;
    }

    public bool IsEnoughFreeSoldier(int wantedSoldierCount, ushort clientId)
    {
      return view.gameManagerVo.playerFeaturesVos[clientId].freeSoldierCount >= wantedSoldierCount;
    }

    public void ChangeCityOwner(int cityId, ushort clientId)
    {
      view.mainMapMediator.view.cities[cityId].ownerID = clientId;

      view.gameManagerVo.playerFeaturesVos[clientId].cities.Add(cityId);
    }

    public void SetCitySoldierCount(int cityId, int soldierCount)
    {
      view.mainMapMediator.view.cities[cityId].soldierCount = soldierCount;
    }

    public void IncreaseCitySoldierCount(int cityId, int increaseCount, ushort clientId)
    {
      view.mainMapMediator.view.cities[cityId].soldierCount += increaseCount;

      DecreaseFreeSoldierCount(clientId, increaseCount);
    }

    public void DecreaseFreeSoldierCount(ushort clientId, int soldierCount)
    {
      view.gameManagerVo.playerFeaturesVos[clientId].freeSoldierCount -= soldierCount;
    }

    public List<int> GetPlayerCities(ushort clientId)
    {
      return view.gameManagerVo.playerFeaturesVos[clientId].cities;
    }

    private void ChangeGameState(GameStateKey gameStateKey)
    {
      view.gameManagerVo.gameStateVo.gameStateKey = gameStateKey;
      SendPacketToLobbyVo<GameStateVo> gameStateVo = networkManager.SetSendPacketToLobbyVo(view.gameManagerVo.gameStateVo, view.lobbyVo.clients);
      dispatcher.Dispatch(MainGameEvent.ChangeGameState, gameStateVo);
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