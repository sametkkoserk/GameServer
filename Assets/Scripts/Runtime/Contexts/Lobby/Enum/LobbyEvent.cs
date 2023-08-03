namespace Runtime.Contexts.Lobby.Enum
{
    public enum LobbyEvent
    {
        SendLobbies,

        CreateLobby,
        
        JoinedToLobby,
        JoinLobby,
        LobbyIsClosed,
        
        QuitFromLobby,
        QuitFromLobbyDone,
        HostQuitFromLobbyDone,

        PlayerReady,
        PlayerReadyResponse
    }
}