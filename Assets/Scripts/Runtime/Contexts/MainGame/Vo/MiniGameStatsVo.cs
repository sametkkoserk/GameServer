using System.Collections.Generic;
using ProtoBuf;
using Runtime.Contexts.Lobby.Vo;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class MiniGameStatsVo
  {
    [ProtoMember(1)]
    public ushort playerId;
    
    [ProtoMember(2)]
    public ushort playerArrangement;

    [ProtoMember(3)]
    public List<string> playerRewards;

    [ProtoMember(4)]
    public PlayerFeaturesVo playerFeaturesVo;
    
    [ProtoIgnore]
    public Dictionary<ushort, ClientVo> clients;
  }
}