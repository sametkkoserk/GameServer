using System.Linq;
using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Vo;
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
    
    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(LobbyEvent.JoinLobby, OnJoinLobby);
      dispatcher.AddListener(LobbyEvent.QuitFromLobby, OnQuitFromLobby);
      dispatcher.AddListener(LobbyEvent.PlayerReady, OnReady);
      dispatcher.AddListener(NetworkEvent.ClientDisconnected,OnClientDisconnected);
    }

    private void OnClientDisconnected(IEvent payload)
    {
      ushort id = (ushort)payload.data;
      if (!view.lobbyVo.clients.ContainsKey(id))
        return;
      QuitFromLobbyVo quitFromLobbyVo = new QuitFromLobbyVo()
      {
        id = id,
        clients = view.lobbyVo.clients,
      };
      OnQuit(quitFromLobbyVo);
    }

    private void Start()
    {
      view.lobbyVo = lobbyModel.createdLobbyVo;

      Debug.Log("lobby Inited");

      OnJoin(view.lobbyVo.hostId);
    }
    
    private void OnReady(IEvent payload)
    {
      PlayerReadyResponseVo playerReadyResponseVo = (PlayerReadyResponseVo)payload.data;

      if (playerReadyResponseVo.lobbyCode != view.lobbyVo.lobbyCode)
        return;
      if (view.lobbyVo.clients[playerReadyResponseVo.id].ready)
        return;
      
      view.lobbyVo.clients[playerReadyResponseVo.id].ready = true;
      view.lobbyVo.readyCount += 1;
      playerReadyResponseVo.startGame = view.lobbyVo.readyCount == view.lobbyVo.playerCount;
      view.lobbyVo.isStarted = playerReadyResponseVo.startGame;
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
        playerColor = new PlayerColorVo(ColorGenerator()),
        userName = playerModel.userList[id].username
      };

      view.lobbyVo.playerCount++;
      
      view.lobbyVo.clients[clientVo.id] = clientVo;

      JoinedToLobbyVo joinedToLobbyVo = new()
      {
        lobbyVo = view.lobbyVo,
        clientVo = clientVo
      };
      
      dispatcher.Dispatch(LobbyEvent.JoinedToLobby, joinedToLobbyVo);
    }

    private void OnQuitFromLobby(IEvent payload)
    {
      QuitFromLobbyVo quitFromLobbyVo = (QuitFromLobbyVo)payload.data;
      
      if (quitFromLobbyVo.lobbyCode != view.lobbyVo.lobbyCode)
        return;

      OnQuit(quitFromLobbyVo);
    }

    public void OnQuit(QuitFromLobbyVo quitFromLobbyVo)
    {
      Debug.Log(quitFromLobbyVo.id);

      if (view.lobbyVo.clients[quitFromLobbyVo.id].ready)
      {
        view.lobbyVo.readyCount -= 1;
      }

      view.lobbyVo.playerCount -= 1;
      view.lobbyVo.clients.Remove(quitFromLobbyVo.id);
      
      quitFromLobbyVo.clients = view.lobbyVo.clients;

      if (view.lobbyVo.playerCount > 0)
      {
        if (quitFromLobbyVo.id == view.lobbyVo.hostId)
        {
          view.lobbyVo.hostId = view.lobbyVo.clients.ElementAt(0).Value.id;
          quitFromLobbyVo.hostId = view.lobbyVo.hostId;
        }

        lobbyModel.lobbies[quitFromLobbyVo.lobbyCode].clients = quitFromLobbyVo.clients;
        lobbyModel.lobbies[quitFromLobbyVo.lobbyCode].hostId = quitFromLobbyVo.hostId;
        
        dispatcher.Dispatch(LobbyEvent.QuitFromLobbyDone, quitFromLobbyVo);
        return;
      }
      
      DebugX.Log(DebugKey.Server, "The lobby was closed because there was no one left in the lobby. Lobby Code: " + view.lobbyVo.lobbyCode);

      lobbyModel.DeleteLobby(view.lobbyVo.lobbyCode);
      Destroy(gameObject);
    }

    private Color ColorGenerator()
    {
      float r = Random.Range(0, 1f);
      float g = Random.Range(0, 1f);
      float b = Random.Range(0, 1f);

      Color color = new(r, g, b);
      return color;
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(LobbyEvent.JoinLobby, OnJoinLobby);
      dispatcher.RemoveListener(LobbyEvent.QuitFromLobby, OnQuitFromLobby);
      dispatcher.RemoveListener(LobbyEvent.PlayerReady, OnReady);
      dispatcher.RemoveListener(NetworkEvent.ClientDisconnected,OnClientDisconnected);

    }
  }
}