using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Lobby.Vo;
using UnityEngine;

namespace Lobby.Model.LobbyModel
{
    public class LobbyModel : ILobbyModel
    {
        public ushort lobbyCount{ get; set; }
        public Dictionary<ushort, LobbyVo> lobbies{ get; set; }
        public LobbyVo createdLobbyVo{ get; set; }
        [PostConstruct]
        public void OnPostContruct()
        {
            lobbies = new Dictionary<ushort, LobbyVo>();
        }
        public void NewLobbyCreated(LobbyVo vo)
        {
            createdLobbyVo = vo;
            lobbies[lobbyCount] = vo;
            lobbyCount += 1;
            Debug.Log("model process completed");
        }
    }
}