using System.Collections.Generic;
using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class GameManagerVo
  {
    [ProtoMember(1)]
    public List<ushort> queueList = new();
    
    [ProtoMember(2)]
    public int queue;

    [ProtoMember(3)]
    public TurnVo turnVo = new();

    [ProtoMember(4)]
    public GameStateVo gameStateVo = new();

    [ProtoMember(5)]
    public PlayerActionVo playerActionVo = new();

    [ProtoMember(6)]
    public Dictionary<ushort, PlayerFeaturesVo> playerFeaturesVos = new();

    [ProtoIgnore]
    public bool startTimer;

    [ProtoIgnore]
    public bool armingFinished;

    [ProtoIgnore]
    public bool attackFinished;
    
    [ProtoIgnore]
    public bool fortifyFinished;

    [ProtoIgnore]
    public Dictionary<int, int> uncompletedAttackCityVos = new();

    [ProtoIgnore]
    public ushort clientId;
  }
}