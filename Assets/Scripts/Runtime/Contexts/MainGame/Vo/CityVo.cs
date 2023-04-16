using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class CityVo
  {
    [ProtoMember(1)]
    public int ID;
    [ProtoMember(2)]
    public bool isPlayable;
    [ProtoMember(3)]
    public List<ushort> neighbors;
    [ProtoMember(4)]
    public int ownerID;
    [ProtoMember(5)]
    public Vector3 position;
    [ProtoMember(6)]
    public int soldierCount;
  }
}