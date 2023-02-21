using System.Collections.Generic;
using Runtime.Lobby.Vo;

namespace Runtime.Lobby.Model.LobbyModel
{
    public interface ILobbyModel
    {
        ushort lobbyCount { get; set; }
        Dictionary<ushort, LobbyVo> lobbies{ get; set; }
        LobbyVo createdLobbyVo{ get; set; }
        void NewLobbyCreated(LobbyVo vo);
    }
}