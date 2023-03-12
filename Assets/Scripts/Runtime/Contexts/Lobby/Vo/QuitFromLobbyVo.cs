using System.Collections.Generic;
using Newtonsoft.Json;

namespace Runtime.Contexts.Lobby.Vo
{
  public class QuitFromLobbyVo
  {
    public ushort clientId;
    public ushort lobbyId;
    public ushort inLobbyId;
    [JsonIgnore]
    public Dictionary<ushort, ClientVo> clients;
  }
}