using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class SceneReadyVo
  {
    [ProtoMember(1)]
    public string lobbyCode;

    [ProtoIgnore]
    public ushort id;
  }
}