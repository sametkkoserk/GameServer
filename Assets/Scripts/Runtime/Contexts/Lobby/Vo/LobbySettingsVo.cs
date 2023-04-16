using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class LobbySettingsVo
  {
    [ProtoMember(1)]
    public ushort lobbyId;
    [ProtoMember(2)]
    public float turnTime;
  }
}