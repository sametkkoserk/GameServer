using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using strange.extensions.mediation.impl;

namespace Runtime.Contexts.Lobby.View.Lobby
{
    public class LobbyView : EventView
    {
        public LobbyVo lobbyVo;
        public ushort lobbyId;
        public string lobbyName;
        public bool isPrivate;
        public ushort leaderId;
        public ushort maxPlayerCount;
        public ushort playerCount;
        
        public List<ClientVo> clients;




    }
}