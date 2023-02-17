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
            joinedToLobbyVo.clientId = id;
            dispatcher.Dispatch(LobbyEvent.JoinedToLobby,joinedToLobbyVo);
        }

        public override void OnRemove()
        {
            dispatcher.RemoveListener(LobbyEvent.JoinLobby,OnJoinLobby);
        }
    }
}