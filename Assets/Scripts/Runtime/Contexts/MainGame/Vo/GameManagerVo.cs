using System.Collections.Generic;
using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class GameManagerVo
  {
    [ProtoMember(2)]
    public int queue;
    
    [ProtoMember(1)]
    public List<ushort> queueList = new();
    
    [ProtoMember(3)]
    public TurnVo turnVo = new();

    [ProtoMember(4)]
    public GameStateVo gameStateVo = new();

    [ProtoMember(5)]
    public PlayerActionVo playerActionVo = new();
  }
}