using ProtoBuf;
using UnityEngine;

namespace Runtime.Contexts.Network.Vo
{
    [ProtoContract]
    public class QuaternionVo
    {
        [ProtoMember(1)]
        public float x;

        [ProtoMember(2)]
        public float y;

        [ProtoMember(3)]
        public float z;
        
        [ProtoMember(4)]
        public float w;
    
        public QuaternionVo(){}
        public QuaternionVo(Quaternion quaternion)
        {
            x = quaternion.x;
            y = quaternion.y;
            z = quaternion.z;
            w = quaternion.w;
        }
    
        public Quaternion ToQuaternion()
        {
            return new Quaternion(x, y, z,w);
        }
    }
}