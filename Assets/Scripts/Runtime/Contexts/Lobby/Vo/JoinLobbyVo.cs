using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class JoinLobbyVo
  {
    [ProtoMember(1)]
    public ushort clientId;
    [ProtoMember(2)]
    public string lobbyCode;
  }
}