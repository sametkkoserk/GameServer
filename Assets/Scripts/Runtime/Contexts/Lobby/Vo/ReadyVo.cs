using ProtoBuf;
using Runtime.Contexts.Network.Vo;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class ReadyVo
  {
    [ProtoMember(1)]
    public string lobbyCode;
  }
}