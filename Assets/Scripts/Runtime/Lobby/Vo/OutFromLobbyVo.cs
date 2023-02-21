using System.Collections.Generic;
using Newtonsoft.Json;

namespace Runtime.Lobby.Vo
{
  public class OutFromLobbyVo
  {
    public ushort clientId;
    public ushort lobbyId;
    public ushort inLobbyId;
    [JsonIgnore]
    public Dictionary<ushort,ClientVo> clients;
  }
}