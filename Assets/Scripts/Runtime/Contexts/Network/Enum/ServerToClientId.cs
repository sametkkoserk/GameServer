namespace Runtime.Contexts.Network.Enum
{
  /// <summary>
  /// Integer value automatically attached, there is no necessary manually set. Also, it must be same with file in the client.
  /// </summary>
  public enum ServerToClientId : ushort
  {
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
    
    UpdateCity,
    ChangePlayerFeature,
    
    MiniGameRewards,
    OpenMainGamePanel,
    
    Attack,
    Fortify,
    CreateMiniGameScene,
    MiniGameCreated,
    SendMiniGameState,
    MiniGaneEnded
  }
}