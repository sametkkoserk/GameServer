using System.Collections.Generic;
using ProtoBuf;
using Runtime.Contexts.Network.Vo;
using UnityEngine;

namespace Runtime.Contexts.MiniGames.Vo
{
    [ProtoContract]
    public class MiniGameStateVo
    {
        [ProtoMember(1)]
        public Dictionary<string, Vector3Vo> changedPositions;
        [ProtoMember(2)]
        public Dictionary<string, QuaternionVo> changedRotations;
        
        [ProtoMember(3)]
        public Dictionary<string, Vector3Vo> newPositions;
        [ProtoMember(4)]
        public Dictionary<string, QuaternionVo> newRotations;
        
        [ProtoMember(5)]
        public List<string> removedObjs;
        
        [ProtoMember(6)]
        public Dictionary<string, Vector3Vo> newTargets;
        
        [ProtoMember(7)]
        public Dictionary<ushort, Vector3Vo> playerPositions;
        [ProtoMember(8)]
        public Dictionary<ushort, QuaternionVo> playerRotations;
    }
}