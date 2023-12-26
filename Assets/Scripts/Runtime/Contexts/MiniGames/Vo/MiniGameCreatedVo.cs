using ProtoBuf;
using UnityEngine;

namespace Runtime.Contexts.MiniGames.Vo
{
    [ProtoContract]
    public class MiniGameCreatedVo
    {
        [ProtoMember(1)]
        public string miniGameKey;
    }
}