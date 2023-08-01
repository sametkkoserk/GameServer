using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class PlayerReadyVo
  {
    [ProtoMember(1)]
    public ushort id;
    [ProtoMember(2)]
    public ushort lobbyId;
  }
}