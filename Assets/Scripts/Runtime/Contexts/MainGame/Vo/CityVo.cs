using System.Collections.Generic;
using ProtoBuf;
using Runtime.Contexts.Network.Vo;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class CityVo
  {
    [ProtoIgnore]
    public ushort clientId;
    
    [ProtoMember(1)]
    public int ID;
    
    [ProtoMember(2)]
    public bool isPlayable;
    
    [ProtoMember(3)]
    public List<ushort> neighbors;
    
    [ProtoMember(4)]
    public ushort ownerID;
    
    [ProtoMember(5)]
    public Vector3Vo position;
    
    [ProtoMember(6)]
    public int soldierCount;
  }
}