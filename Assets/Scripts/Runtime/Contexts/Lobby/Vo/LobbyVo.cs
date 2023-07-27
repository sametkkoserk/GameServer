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
        public ushort leaderId;
        [ProtoMember(4)]
        public string lobbyCode;
        [ProtoMember(5)]
        public string lobbyName;
        [ProtoMember(6)]
        public LobbySettingsVo lobbySettingsVo;
        [ProtoMember(7)]
        public ushort maxPlayerCount;
        [ProtoMember(8)]
        public ushort playerCount;
        [ProtoMember(9)]
        public ushort readyCount;
    }
}