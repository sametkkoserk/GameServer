using System.Collections.Generic;

namespace Lobby.Vo
{
  public class PlayerReadyVo
  {
    public ushort clientId;
    public ushort lobbyId;
    public ushort inLobbyId;
    public bool startGame;
    public List<ClientVo> clients;
  }
}