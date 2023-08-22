using System.Collections;
using System.Linq;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using strange.extensions.dispatcher.eventdispatcher.api;
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

    public override void OnRegister()
    {
    }

    /// <summary>It checks everyone see the main map and ready to start. If each player ready, method will determine queue of players.</summary>
    /// <param name="payload">GameStartVo.</param>
    private void Start()
    {
      view.lobbyVo =mainGameModel.managerLobbyVos[0];
      mainGameModel.managerLobbyVos.RemoveAt(0);
      
      // Random rnd = new();
      // view.queueList = Enumerable.Range(0, view.lobbyVo.playerCount).OrderBy(x => rnd.Next()).Take(view.lobbyVo.playerCount).ToList();

      for (int i = 0; i < view.lobbyVo.playerCount; i++)
      {
        view.queueList.Add(view.lobbyVo.clients.ElementAt(i).Value.id);
      }

      view.queue = -1;
      view.turnVo.clientVos = view.lobbyVo.clients;
      NextTurn();
      // sırayı ayarladık. İlk oyuncu başlayacak süreyi sunucu tutaack bittiğinde yeni oyuncuya dispatch atacak.
    }

    private void NextTurn()
    {
      // In the future connection information must be checked. If player is AFK, it will be the next player's turn.
      view.queue++;
      if (view.queue >= view.queueList.Count)
        view.queue = 0;
      
      view.turnVo.id = view.queueList.ElementAt(view.queue);
      view.turnVo.remainingTime = view.lobbyVo.lobbySettingsVo.turnTime;
      
      dispatcher.Dispatch(MainGameEvent.NextTurn, view.turnVo);

      StartCoroutine(SetTimer());
    }
    
    private IEnumerator SetTimer()
    {
      yield return new WaitForSeconds(1f);
      
      if (view.turnVo.remainingTime <= 0f)
      {
        NextTurn();
        yield break;
      }
      
      view.turnVo.remainingTime--;
      
      dispatcher.Dispatch(MainGameEvent.SendRemainingTime, view.turnVo);

      StartCoroutine(SetTimer());
    }

    public override void OnRemove()
    {
    }
  }
}