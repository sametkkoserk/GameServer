namespace Runtime.Contexts.Network.Enum
{
  public enum ServerToClientId : ushort
  {
    //Integer value automatically attached, there is no necessary manually set. Also, it must be same with file in the client.
    RegisterAccepted,
    
    JoinedToLobby,
    NewPlayerToLobby,
    SendLobbies,
    
    QuitFromLobbyDone,
    PlayerReadyResponse,
    GameSettingsChanged,
    LobbyIsClosed,
    
    GameStartPreparations,
    
    NextTurn,
    RemainingTime,
    
    GameStateChanged,
    PlayerActionChanged,
    SendPlayerActionReference,
    
    SendClaimedCity,
  }
}