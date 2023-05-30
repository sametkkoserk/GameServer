namespace Runtime.Contexts.Network.Enum
{
  public enum ServerToClientId : ushort
  {
    //Integer value automatically attached, there is no necessary manually set. Also, it must be same with file in the server.
    Response,
    RegisterAccepted,
    JoinedToLobby,
    NewPlayerToLobby,
    SendLobbies,
    QuitFromLobbyDone,
    PlayerReadyResponse,
    GameSettingsChanged,
    GameStartPreparations,
    SendUserLobbyID,
    SendTurn
  }
}