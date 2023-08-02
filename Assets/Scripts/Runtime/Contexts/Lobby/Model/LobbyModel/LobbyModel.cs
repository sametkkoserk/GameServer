using System.Collections.Generic;
using System.Linq;
using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Lobby.Vo;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Model.LobbyModel
{
    public class LobbyModel : ILobbyModel
    {
        public ushort lobbyCount { get; set; }
        public Dictionary<string, LobbyVo> lobbies{ get; set; }
        
        public LobbyVo createdLobbyVo{ get; set; }
        
        
        [PostConstruct]
        public void OnPostConstruct()
        {
            lobbies = new Dictionary<string, LobbyVo>();
        }
        
        public void NewLobbyCreated(LobbyVo vo)
        {
            createdLobbyVo = vo;
            createdLobbyVo.lobbyCode = GenerateLobbyCode(2);
            lobbies.Add(createdLobbyVo.lobbyCode, vo);
            lobbyCount += 1;
            
            DebugX.Log(DebugKey.Server, "New lobby is created.");
        }

        public void DeleteLobby(string lobbyCode)
        {
            lobbies.Remove(lobbyCode);
            lobbyCount--;
        }
        
        private string GenerateLobbyCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string randomString = "";

            for (int i = 0; i < length; i++)
            {
                randomString += chars[Random.Range(0, chars.Length)];
            }

            if (lobbies.ContainsKey(randomString))
                GenerateLobbyCode(length);
            
            return randomString;
        }
    }
}