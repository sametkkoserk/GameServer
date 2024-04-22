using ProtoBuf;
using UnityEngine;

namespace Runtime.Contexts.MiniGames.Vo
{
    [ProtoContract]
    public class MiniGameSceneReadyVo
    {
        [ProtoMember(1)]
        public string lobbyCode;
    }
}