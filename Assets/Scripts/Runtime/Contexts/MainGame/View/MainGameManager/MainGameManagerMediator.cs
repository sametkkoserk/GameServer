using System.Linq;
using Riptide;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using Random = System.Random;

namespace Runtime.Contexts.MainGame.View.MainGameManager
{
  public class MainGameManagerMediator : EventMediator
  {
    [Inject]
    public MainGameManagerView view { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }
    
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.GameStart, OnGameStartCheck);
      
      dispatcher.AddListener(ClientToServerId.NextTurn, TurnEnded);
    }

    /// <summary>It checks everyone see the main map and ready to start. If each player ready, method will determine queue of players.</summary>
    /// <param name="payload">GameStartVo.</param>
    private void OnGameStartCheck(IEvent payload)
    {
      GameStartVo vo = (GameStartVo)payload.data;

      view.lobbyVo = lobbyModel.lobbies[vo.lobbyCode];

      view.readyCount++;

      if (view.lobbyVo == null || view.readyCount < view.lobbyVo.playerCount)
        return;

      Random rnd = new();
      view.queueList = Enumerable.Range(0, view.readyCount).OrderBy(x => rnd.Next()).Take(view.readyCount).ToList();
      
      NextTurn();
    }

    private void NextTurn()
    {
      //TODO Şafak
      // In the future connection information must be checked. If player is AFK, it will be the next player's turn.
      for (int i = 0; i < view.lobbyVo.clients.Count; i++)
      {
        //if (view.lobbyVo.clients.ElementAt(i).Value.inLobbyId != view.queueList[view.queue])) continue;
        
        // Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.SendTurn);
        // message = networkManager.SetData(message, view.lobbyVo.clients.ElementAt(i).Value.inLobbyId);
        //
        // networkManager.Server.Send(message, view.lobbyVo.clients.ElementAt(i).Value.id);
        //
        // view.queue++;
        // if (view.queue >= view.queueList.Count)
        // {
        //   view.queue = 0;
        // }
        // return;
      }
    }

    private void TurnEnded()
    {
      NextTurn();
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.GameStart, OnGameStartCheck);
      
      dispatcher.RemoveListener(ClientToServerId.NextTurn, TurnEnded);
    }
  }
}