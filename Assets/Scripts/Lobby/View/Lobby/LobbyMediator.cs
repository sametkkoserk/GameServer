using System;
using Lobby.Enum;
using Lobby.Model.LobbyModel;
using Lobby.Vo;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Lobby.View.Lobby
{
    public class LobbyMediator : EventMediator
    {
        [Inject] 
        public LobbyView view { get; set; }
        
        [Inject] 
        public ILobbyModel lobbyModel { get; set; }

        public override void OnRegister()
        {
            dispatcher.AddListener(LobbyEvent.JoinLobby,OnJoinLobby);
            dispatcher.AddListener(LobbyEvent.OutFromLobby,OnOutFromLobby);
            dispatcher.AddListener(LobbyEvent.PlayerReady,OnReady);
        }

        private void OnReady(IEvent payload)
        {
            PlayerReadyVo playerReadyVo = (PlayerReadyVo)payload.data;
            if (playerReadyVo.lobbyId!=view.lobbyId)
                return;
            view.lobbyVo.clients[playerReadyVo.inLobbyId].ready=true;
            view.lobbyVo.readyCount += 1;
            playerReadyVo.startGame = view.lobbyVo.readyCount == view.lobbyVo.playerCount;
            playerReadyVo.clients = view.lobbyVo.clients;
            dispatcher.Dispatch(LobbyEvent.PlayerReadyResponse,playerReadyVo);
            Debug.Log("player is ready confirmed");

        }


        private void Start()
        {
            view.lobbyVo = lobbyModel.createdLobbyVo;

            Debug.Log("lobby Inited");

            OnJoin(view.lobbyVo.leaderId);

        }
        
        private void OnJoinLobby(IEvent payload)
        {
            JoinLobbyVo joinLobbyVo = (JoinLobbyVo)payload.data;
            if (joinLobbyVo.lobbyId!=view.lobbyId)
                return;
            OnJoin(joinLobbyVo.clientId);
        }

        private void OnJoin(ushort id)
        {
            ClientVo clientVo = new ClientVo();
            clientVo.id = id;
            clientVo.inLobbyId = view.lobbyVo.playerCount;
            clientVo.colorId = view.lobbyVo.playerCount;
            
            view.lobbyVo.playerCount += 1;
            view.lobbyVo.clients.Add(clientVo);
            
            JoinedToLobbyVo joinedToLobbyVo = new JoinedToLobbyVo();
            joinedToLobbyVo.lobby = view.lobbyVo;
            joinedToLobbyVo.clientVo = clientVo;
            dispatcher.Dispatch(LobbyEvent.JoinedToLobby,joinedToLobbyVo);
            
            
            
        }
        
        
        private void OnOutFromLobby(IEvent payload)
        {
            OutFromLobbyVo outFromLobbyVo = (OutFromLobbyVo)payload.data;
            if (outFromLobbyVo.lobbyId!=view.lobbyId)
                return;
            Debug.Log(outFromLobbyVo.inLobbyId);
            
            if (view.lobbyVo.clients[outFromLobbyVo.inLobbyId].ready)
            {
                view.lobbyVo.readyCount -= 1;
            }
            
            view.lobbyVo.clients.RemoveAt(outFromLobbyVo.inLobbyId);
            view.lobbyVo.playerCount -= 1;
            
            for (ushort i = outFromLobbyVo.inLobbyId; i < view.lobbyVo.playerCount; i++)
            {
                view.lobbyVo.clients[i].inLobbyId = i;
            }
            
            outFromLobbyVo.clients=view.lobbyVo.clients;
            dispatcher.Dispatch(LobbyEvent.OutFromLobbyDone,outFromLobbyVo);
        }

        public override void OnRemove()
        {
            dispatcher.RemoveListener(LobbyEvent.JoinLobby,OnJoinLobby);
            dispatcher.RemoveListener(LobbyEvent.OutFromLobby,OnOutFromLobby);
            dispatcher.RemoveListener(LobbyEvent.PlayerReady,OnReady);
        }
    }
}