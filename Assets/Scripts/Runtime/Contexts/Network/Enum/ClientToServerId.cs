namespace Runtime.Contexts.Network.Enum
{
  public enum ClientToServerId : ushort
  {
    //Integer value automatically attached, there is no necessary manually set. Also, it must be same with file in the server.
    CreateLobby,
    GetLobbies,
    JoinLobby,
    OutFromLobby,
    PlayerReady,
    GameSettingsChanged,
    GameStart,
    NextTurn
  }
}