using Newtonsoft.Json;
using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class PlayerReadyResponseVo
  {
    [ProtoMember(1)]
    public ushort id;
    [ProtoMember(2)]
    public string lobbyCode;
    [ProtoMember(3)]
    public bool startGame;

    [JsonIgnore]
    public LobbyVo lobbyVo;
  }
}