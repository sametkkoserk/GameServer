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
        }

        private void OnJoinLobby(IEvent payload)
        {
            JoinLobbyVo joinLobbyVo = (JoinLobbyVo)payload.data;
            if (joinLobbyVo.lobbyId!=view.lobbyId)
                return;
            view.clients.Add(joinLobbyVo.clientId);
            Debug.Log("Client Added");
            
            JoinedToLobbyVo joinedToLobbyVo = new JoinedToLobbyVo();
            joinedToLobbyVo.lobby = view.lobbyVo;
            joinedToLobbyVo.clientId = joinLobbyVo.clientId;
            dispatcher.Dispatch(LobbyEvent.JoinedToLobby,joinedToLobbyVo);
        }

        private void Start()
        {
            
            LobbyVo vo = lobbyModel.createdLobbyVo;
            view.lobbyVo = vo;
            view.lobbyId = vo.lobbyId;
            view.lobbyName = vo.lobbyName;
            view.isPrivate = vo.isPrivate;
            view.leaderId = vo.leaderId;
            Debug.Log("lobby Inited");
            
            view.clients.Add(vo.leaderId);
            
            JoinedToLobbyVo joinedToLobbyVo = new JoinedToLobbyVo();
            joinedToLobbyVo.lobby = vo;
            joinedToLobbyVo.clientId = vo.leaderId;
            dispatcher.Dispatch(LobbyEvent.JoinedToLobby,joinedToLobbyVo);

        }

        public override void OnRemove()
        {
        }
    }
}