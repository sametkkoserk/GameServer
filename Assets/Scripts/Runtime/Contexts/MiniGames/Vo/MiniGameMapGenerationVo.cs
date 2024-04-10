using System.Collections.Generic;
using ProtoBuf;
using Runtime.Contexts.Network.Vo;
using UnityEngine;

namespace Runtime.Contexts.MiniGames.Vo
{
    [ProtoContract]
    public class MiniGameMapGenerationVo
    {
        [ProtoMember(1)]
        public List<KeyValuePair<int,int>> mapItems;
        
        [ProtoMember(2)]
        public List<Vector3Vo> positions;
        
        [ProtoMember(3)]
        public List<QuaternionVo> rotations;
    }
}