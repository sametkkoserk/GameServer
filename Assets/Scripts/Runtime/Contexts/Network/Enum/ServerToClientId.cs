namespace Runtime.Contexts.Network.Enum
{
  public enum ServerToClientId : ushort
  {
    RESPONSE = 1,
    JoinedToLobby = 2,
    NewPlayerToLobby = 3,
    SendLobbies = 4,
    OutFromLobbyDone = 5,
    PlayerReadyResponse = 6,
    SendMap = 7,
    SendUserLobbyID = 8,
    SendTurn = 9
  }
}