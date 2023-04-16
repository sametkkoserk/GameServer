using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class JoinedToLobbyVo
  {
    [ProtoMember(1)]
    public ClientVo clientVo;
    [ProtoMember(2)]
    public LobbyVo lobby;
  }
}