using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;

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
      dispatcher.AddListener(MainGameEvent.PlayerActionEnded, ChangeTurn);
    }

    /// <summary>It checks everyone see the main map and ready to start. If each player ready, method will determine queue of players.</summary>
    private void Start()
    {
      // Random rnd = new();
      // view.queueList = Enumerable.Range(0, view.lobbyVo.playerCount).OrderBy(x => rnd.Next()).Take(view.lobbyVo.playerCount).ToList();
      
      view.lobbyVo = mainGameModel.managerLobbyVos[0];
      mainGameModel.managerLobbyVos.RemoveAt(0);

      for (int i = 0; i < view.lobbyVo.playerCount; i++)
      {
        view.gameManagerVo.queueList.Add(view.lobbyVo.clients.ElementAt(i).Value.id);
      }
      view.gameManagerVo.queue = -1;

      view.gameManagerVo.gameStateVo.gameStateKey = GameStateKey.ClaimCity;
      SendPacketToLobbyVo<GameStateVo> gameStateVo = networkManager.SetSendPacketToLobbyVo(view.gameManagerVo.gameStateVo, view.lobbyVo.clients);
      dispatcher.Dispatch(MainGameEvent.ChangeGameState, gameStateVo);

      List<PlayerActionKey> playerActionKey = new() { PlayerActionKey.OpenDetailsPanel };
      for (int i = 0; i < view.lobbyVo.clients.Count; i++)
        view.gameManagerVo.playerActionVo.playerActionKeys.Add(view.lobbyVo.clients.ElementAt(i).Value.id, playerActionKey);
      dispatcher.Dispatch(MainGameEvent.ChangePlayerAction, view.gameManagerVo.playerActionVo);
      
      NextTurn(true);
    }

    #region Turn

    private void NextTurn(bool setTimer)
    {
      // In the future connection information must be checked. If player is AFK, it will be the next player's turn.
      if (view.gameManagerVo.queue >= 0)
      {
        ushort id = view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue);
        UpdatePlayerActionKeys(id, PlayerActionKey.ClaimCity, false);
      }
      
      view.gameManagerVo.queue++;
      if (view.gameManagerVo.queue >= view.gameManagerVo.queueList.Count)
        view.gameManagerVo.queue = 0;
      
      view.gameManagerVo.turnVo.id = view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue);
      view.gameManagerVo.turnVo.remainingTime = view.lobbyVo.lobbySettingsVo.turnTime;

      SendPacketToLobbyVo<TurnVo> vo = networkManager.SetSendPacketToLobbyVo(view.gameManagerVo.turnVo, view.lobbyVo.clients); 
      dispatcher.Dispatch(MainGameEvent.NextTurn, vo);
      
      ushort nextId = view.gameManagerVo.queueList.ElementAt(view.gameManagerVo.queue);
      UpdatePlayerActionKeys(nextId, PlayerActionKey.ClaimCity, true);

      if (setTimer)
        StartCoroutine(SetTimer());
    }

    public void ChangeTurn()
    {
      NextTurn(false);
    }
    
    private IEnumerator SetTimer()
    {
      yield return new WaitForSeconds(1f);
      
      if (view.gameManagerVo.turnVo.remainingTime <= 0f)
      {
        NextTurn(true);
        yield break;
      }
      
      view.gameManagerVo.turnVo.remainingTime--;
      
      SendPacketToLobbyVo<TurnVo> vo = networkManager.SetSendPacketToLobbyVo(view.gameManagerVo.turnVo, view.lobbyVo.clients);
      dispatcher.Dispatch(MainGameEvent.SendRemainingTime, vo);

      StartCoroutine(SetTimer());
    }
    
    #endregion

    #region PlayerActions
    private void UpdatePlayerActionKeys(ushort id, PlayerActionKey playerActionKey, bool add)
    {
      List<PlayerActionKey> playerActionKeys = view.gameManagerVo.playerActionVo.playerActionKeys[id];
      
      if (add)
      {
        playerActionKeys.Add(playerActionKey);
      }
      else
      {
        if (playerActionKeys.Contains(playerActionKey))
          playerActionKeys.Remove(playerActionKey);
      }
      
      Dictionary<ushort, List<PlayerActionKey>> newList = new() { { id, playerActionKeys } };
      PlayerActionVo vo = new() { playerActionKeys = newList };
      
      dispatcher.Dispatch(MainGameEvent.ChangePlayerAction, vo);
    }
    #endregion

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.PlayerActionEnded, ChangeTurn);
    }
  }
}