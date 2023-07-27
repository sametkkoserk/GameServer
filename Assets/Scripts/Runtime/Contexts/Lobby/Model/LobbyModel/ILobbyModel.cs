using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;

namespace Runtime.Contexts.Lobby.Model.LobbyModel
{
    public interface ILobbyModel
    {
        Dictionary<string, LobbyVo> lobbies{ get; set; }
        LobbyVo createdLobbyVo{ get; set; }
        void NewLobbyCreated(LobbyVo vo);
    }
}