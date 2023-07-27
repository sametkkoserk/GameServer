using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.Lobby.View.Lobby
{
  public class LobbyMediator : EventMediator
  {
    [Inject]
    public LobbyView view { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(LobbyEvent.JoinLobby, OnJoinLobby);
      dispatcher.AddListener(LobbyEvent.QuitFromLobby, OnQuitFromLobby);
      dispatcher.AddListener(LobbyEvent.PlayerReady, OnReady);
    }

    private void Start()
    {
      view.lobbyVo = lobbyModel.createdLobbyVo;

      Debug.Log("lobby Inited");

      OnJoin(view.lobbyVo.leaderId);
    }
    
    private void OnReady(IEvent payload)
    {
      PlayerReadyResponseVo playerReadyResponseVo = (PlayerReadyResponseVo)payload.data;

      if (playerReadyResponseVo.lobbyCode != view.lobbyVo.lobbyCode)
        return;

      view.lobbyVo.clients[playerReadyResponseVo.inLobbyId].ready = true;
      view.lobbyVo.readyCount += 1;
      playerReadyResponseVo.startGame = view.lobbyVo.readyCount == view.lobbyVo.playerCount;
      playerReadyResponseVo.lobbyVo = view.lobbyVo;

      dispatcher.Dispatch(LobbyEvent.PlayerReadyResponse, playerReadyResponseVo);
      Debug.Log("player is ready confirmed");
    }

    private void OnJoinLobby(IEvent payload)
    {
      JoinLobbyVo joinLobbyVo = (JoinLobbyVo)payload.data;
      if (joinLobbyVo.lobbyCode != view.lobbyVo.lobbyCode)
        return;
      OnJoin(joinLobbyVo.clientId);
    }

    private void OnJoin(ushort id)
    {
      ClientVo clientVo = new()
      {
        id = id,
        inLobbyId = view.lobbyVo.playerCount,
        colorId = view.lobbyVo.playerCount
      };

      view.lobbyVo.playerCount += 1;
      view.lobbyVo.clients[clientVo.inLobbyId] = clientVo;

      JoinedToLobbyVo joinedToLobbyVo = new()
      {
        lobby = view.lobbyVo,
        clientVo = clientVo
      };
      dispatcher.Dispatch(LobbyEvent.JoinedToLobby, joinedToLobbyVo);
    }


    private void OnQuitFromLobby(IEvent payload)
    {
      QuitFromLobbyVo quitFromLobbyVo = (QuitFromLobbyVo)payload.data;
      
      if (quitFromLobbyVo.lobbyCode != view.lobbyVo.lobbyCode)
        return;
      
      Debug.Log(quitFromLobbyVo.inLobbyId);

      if (view.lobbyVo.clients[quitFromLobbyVo.inLobbyId].ready)
      {
        view.lobbyVo.readyCount -= 1;
      }

      view.lobbyVo.playerCount -= 1;

      for (ushort i = quitFromLobbyVo.inLobbyId; i < view.lobbyVo.playerCount; i++)
      {
        view.lobbyVo.clients[i] = view.lobbyVo.clients[(ushort)(i + 1)];
        view.lobbyVo.clients[i].inLobbyId = i;
      }

      view.lobbyVo.clients.Remove(view.lobbyVo.playerCount);

      quitFromLobbyVo.clients = view.lobbyVo.clients;
      
      dispatcher.Dispatch(LobbyEvent.QuitFromLobbyDone, quitFromLobbyVo);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(LobbyEvent.JoinLobby, OnJoinLobby);
      dispatcher.RemoveListener(LobbyEvent.QuitFromLobby, OnQuitFromLobby);
      dispatcher.RemoveListener(LobbyEvent.PlayerReady, OnReady);
    }
  }
}