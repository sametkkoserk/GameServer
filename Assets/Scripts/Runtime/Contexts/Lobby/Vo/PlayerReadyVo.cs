using System.Collections.Generic;
using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class PlayerReadyVo
  {
    [ProtoMember(1)]
    public Dictionary<ushort, ClientVo> clients;
    
    [ProtoMember(2)]
    public string lobbyCode;
    
    [ProtoMember(3)]
    public bool startGame;
    
    [ProtoMember(4)]
    public ushort readyCount;

  }
}