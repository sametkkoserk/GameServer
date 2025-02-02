using System.Collections.Generic;
using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class GameManagerVo
  {
    [ProtoIgnore]
    public ushort clientId;
    
    [ProtoMember(1)]
    public List<ushort> queueList = new();
    
    [ProtoMember(2)]
    public int queue;

    [ProtoMember(3)]
    public TurnVo turnVo = new();

    [ProtoMember(4)]
    public GameStateVo gameStateVo = new();

    [ProtoMember(6)]
    public Dictionary<ushort, PlayerFeaturesVo> playerFeaturesVos = new();

    [ProtoIgnore]
    public bool claimCityEnded;

    [ProtoIgnore]
    public int claimCitySoldierCount;

    [ProtoIgnore]
    public bool nextTurn = true;

    [ProtoIgnore]
    public bool startTimer = true;

    [ProtoIgnore]
    public Dictionary<int, int> uncompletedAttackCityVos = new();
  }
}