using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Model.LobbyModel
{
    public interface ILobbyModel
    {
        Dictionary<string, LobbyVo> lobbies{ get; set; }
        
        LobbyVo createdLobbyVo{ get; set; }
        
        void NewLobbyCreated(LobbyVo vo);

        void DeleteLobby(string lobbyCode);

        void UpdateLobby(LobbyVo lobbyVo);

        void OnJoin(string lobbyCode, ClientVo clientVo);

        void OnReady(string lobbyCode, ushort id);
        
        void OnQuit(string lobbyCode, ushort id);
        
        Color ColorGenerator();
    }
}