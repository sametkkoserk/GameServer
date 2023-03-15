using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Model.LobbyModel
{
    public class LobbyModel : ILobbyModel
    {
        public ushort lobbyCount{ get; set; }
        public Dictionary<ushort, LobbyVo> lobbies{ get; set; }
        public LobbyVo createdLobbyVo{ get; set; }
        
        [PostConstruct]
        public void OnPostConstruct()
        {
            lobbies = new Dictionary<ushort, LobbyVo>();
        }
        public void NewLobbyCreated(LobbyVo vo)
        {
            createdLobbyVo = vo;
            createdLobbyVo.lobbyId = lobbyCount;
            lobbies[lobbyCount] = vo;
            lobbyCount += 1;
            Debug.Log("model process completed");
        }
    }
}