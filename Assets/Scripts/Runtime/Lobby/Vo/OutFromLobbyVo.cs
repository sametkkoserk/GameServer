using System.Collections.Generic;

namespace Runtime.Lobby.Vo
{
  public class OutFromLobbyVo
  {
    public ushort clientId;
    public ushort lobbyId;
    public ushort inLobbyId;
    public Dictionary<ushort,ClientVo> clients;
  }
}