using System.Collections.Generic;
using Newtonsoft.Json;
using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class QuitFromLobbyVo
  {
    [ProtoMember(1)]
    public ushort id;

    [ProtoMember(2)]
    public string lobbyCode;
    
    [ProtoMember(3)]
    public Dictionary<ushort, ClientVo> clients;
  }
}