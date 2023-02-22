using System.Collections.Generic;

namespace Runtime.Contexts.Lobby.Vo
{
    public class LobbyVo
    {
        public ushort lobbyId;
        
        public string lobbyName;
        
        public bool isPrivate;
        
        public ushort leaderId;
        
        public ushort maxPlayerCount;
        
        public ushort playerCount;
        
        public ushort readyCount;
        
        public Dictionary<ushort, ClientVo> clients;
    }
}