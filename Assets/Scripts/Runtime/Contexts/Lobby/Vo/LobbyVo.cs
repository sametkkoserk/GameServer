using System.Collections.Generic;
using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
    [ProtoContract]
    public class LobbyVo
    {
        [ProtoMember(1)]
        public Dictionary<ushort, ClientVo> clients;
    
        [ProtoMember(2)]
        public bool isPrivate;
    
        [ProtoMember(3)]
        public bool isStarted;
    
        [ProtoMember(4)]
        public ushort hostId;
    
        [ProtoMember(5)]
        public string lobbyCode;
    
        [ProtoMember(6)]
        public string lobbyName;
    
        [ProtoMember(7)]
        public LobbySettingsVo lobbySettingsVo;
    
        [ProtoMember(8)]
        public ushort maxPlayerCount;
    
        [ProtoMember(9)]
        public ushort playerCount;
    
        [ProtoMember(10)]
        public ushort readyCount;
    }
}