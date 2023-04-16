using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class ClientVo
  {
    [ProtoMember(1)]
    public ushort id;
    [ProtoMember(2)]
    public ushort inLobbyId;
    [ProtoMember(3)]
    public string userName;
    [ProtoMember(4)]
    public ushort colorId;
    [ProtoMember(5)]
    public bool ready;
  }
}