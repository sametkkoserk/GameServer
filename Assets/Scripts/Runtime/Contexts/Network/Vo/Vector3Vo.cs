using ProtoBuf;
using UnityEngine;

namespace Runtime.Contexts.Network.Vo
{
  [ProtoContract]
  public class Vector3Vo
  {
    public Vector3Vo(){}
    public Vector3Vo(Vector3 vector)
    {
      x = vector.x;
      y = vector.y;
      z = vector.z;
    }
    
    public Vector3 ToVector3()
    {
      return new Vector3(x, y, z);
    }
    
    [ProtoMember(1)]
    public float x;

    [ProtoMember(2)]
    public float y;

    [ProtoMember(3)]
    public float z;
  }
}