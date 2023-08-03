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
<<<<<<< HEAD
        public ushort hostId;
=======
        public bool isStarted;
>>>>>>> 3fd00c1de15a069e5477552a72f8e698a412cc43
        
        [ProtoMember(4)]
        public ushort leaderId;
        
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

        [ProtoIgnore]
        public ushort inLobbyIdCounter;
    }
}