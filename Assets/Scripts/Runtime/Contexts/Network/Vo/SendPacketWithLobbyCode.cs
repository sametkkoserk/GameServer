using ProtoBuf;

namespace Runtime.Contexts.Network.Vo
{
  [ProtoContract]
  public class SendPacketWithLobbyCode<T>
  {
    [ProtoMember(1)]
    public T mainClass;

    [ProtoMember(2)]
    public string lobbyCode;
  }
}