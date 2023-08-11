using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class GameStartVo
  {
    [ProtoMember(1)]
    public bool gameStart;
    
    [ProtoMember(2)]
    public string lobbyCode;

    [ProtoIgnore]
    public ushort clientId;
  }
}