using System.Collections.Generic;
using Lobby.Vo;
using strange.extensions.mediation.impl;
using UnityEngine.Serialization;

namespace Lobby.View.Lobby
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