using Newtonsoft.Json;

namespace Runtime.Contexts.Lobby.Vo
{
  public class PlayerReadyResponseVo
  {
    public ushort lobbyId;
    
    public ushort inLobbyId;
    
    public bool startGame;

    [JsonIgnore]
    public LobbyVo lobbyVo;
  }
}