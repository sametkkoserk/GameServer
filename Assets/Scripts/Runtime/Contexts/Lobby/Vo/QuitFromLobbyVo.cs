using System.Collections.Generic;
using Newtonsoft.Json;
using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class QuitFromLobbyVo
  {
    [ProtoMember(1)]
    public ushort clientId;
    
    [ProtoMember(2)]
    public ushort inLobbyId;
    
    [ProtoMember(3)]
    public string lobbyCode;
    
    [JsonIgnore]
    public Dictionary<ushort, ClientVo> clients;
  }
}