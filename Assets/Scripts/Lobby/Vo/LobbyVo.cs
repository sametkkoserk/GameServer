using System.Collections.Generic;
using strange.extensions.mediation.impl;

namespace Lobby.Vo
{
    public class LobbyVo
    {
        public ushort lobbyId;
        public string lobbyName;
        public bool isPrivate;
        public ushort leaderId;
        public ushort maxPlayerCount;
        public ushort playerCount;
        public List<ClientVo> clients;
    }
}