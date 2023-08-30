namespace Runtime.Contexts.MainGame.Enum
{
  public enum MainGameEvent
  {
    PlayerSceneReady,
    CreateMap,
    SendMap,
    GameStart,
    
    NextTurn,
    SendRemainingTime,
    
    SetAllPermissionPlayersAction,
    ChangeGameState,
    ChangePlayerAction,
    
    ClaimCity,
    SendClaimedCity,
    PlayerActionEnded
  }
}
