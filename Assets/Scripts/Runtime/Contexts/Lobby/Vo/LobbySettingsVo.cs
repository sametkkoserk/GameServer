using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class LobbySettingsVo
  {
    [ProtoMember(1)]
    public string lobbyCode;
    [ProtoMember(2)]
    public float turnTime;
  }
}