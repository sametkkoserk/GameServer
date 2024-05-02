using System.Collections.Generic;
using System.Linq;
using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Vo;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;
using UnityEngine.Networking;

namespace Runtime.Contexts.Lobby.Model.LobbyModel
{
    public class LobbyModel : ILobbyModel
    {
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher{ get; set;}
        public ushort lobbyCount { get; set; }
        
        public Dictionary<string, LobbyVo> lobbies{ get; set; }
        
        public LobbyVo createdLobbyVo{ get; set; }

        [PostConstruct]
        public void OnPostConstruct()
        {
            lobbies = new Dictionary<string, LobbyVo>();
        }

        public void DeleteLobby(string lobbyCode)
        {
            lobbies.Remove(lobbyCode);
            lobbyCount--;
        }

        public void UpdateLobby(LobbyVo lobbyVo)
        {
            lobbies[lobbyVo.lobbyCode] = lobbyVo;
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
        
        public void NewLobbyCreated(LobbyVo vo)
        {
            createdLobbyVo = vo;
            createdLobbyVo.lobbyCode = GenerateLobbyCode(2);
            lobbies.Add(createdLobbyVo.lobbyCode, vo);
            lobbyCount += 1;

            DebugX.Log(DebugKey.Server, "New lobby is created. Lobby Code: " + createdLobbyVo.lobbyCode);
        }
        

        public void OnJoin(string lobbyCode,ClientVo clientVo)
        {
            LobbyVo lobbyVo = lobbies[lobbyCode];

            lobbyVo.playerCount++;
            lobbyVo.clients.Add(clientVo.id, clientVo);
            
            JoinedToLobbyVo joinedToLobbyVo = new()
            {
                lobbyVo = lobbyVo,
                clientVo = clientVo
            };
      
            dispatcher.Dispatch(LobbyEvent.JoinedToLobby, joinedToLobbyVo);
        }
        
        public void OnReady(string lobbyCode,ushort id)
        {
            LobbyVo lobbyVo = lobbies[lobbyCode];
            
            if (lobbyVo.clients[id].state == (ushort)ClientState.LobbyReady)
                return;
      
            lobbyVo.clients[id].state=(ushort)ClientState.LobbyReady;
            lobbyVo.readyCount ++;
            
            PlayerReadyVo playerReadyVo = new()
            {
                startGame = lobbyVo.readyCount == lobbyVo.playerCount
            };
            lobbyVo.isStarted = playerReadyVo.startGame;
            playerReadyVo.clients = lobbyVo.clients;
            playerReadyVo.readyCount = lobbyVo.readyCount;
            playerReadyVo.lobbyCode = lobbyVo.lobbyCode;
      
            dispatcher.Dispatch(LobbyEvent.PlayerReadyResponse, playerReadyVo);
      
            DebugX.Log(DebugKey.Lobby, "Player is ready.");
        }

        public void OnQuit(string lobbyCode, ushort id)
        {
            LobbyVo lobbyVo = lobbies[lobbyCode];
            if (lobbyVo.clients[id].state == (ushort)ClientState.LobbyReady)
            {
                lobbyVo.readyCount--;
            }

            QuitFromLobbyVo quitFromLobbyVo = new();

            lobbyVo.playerCount -= 1;
            lobbyVo.clients.Remove(id);
            quitFromLobbyVo.id = id;
            quitFromLobbyVo.clients = lobbyVo.clients;

            if (lobbyVo.playerCount > 0)
            {
                if (id == lobbyVo.hostId)
                {
                    lobbyVo.hostId = lobbyVo.clients.ElementAt(0).Value.id;
                    quitFromLobbyVo.hostId = lobbyVo.hostId;
                }

                dispatcher.Dispatch(LobbyEvent.QuitFromLobbyDone, quitFromLobbyVo);
                return;
            }
      
            dispatcher.Dispatch(LobbyEvent.QuitFromLobbyDone, quitFromLobbyVo);

            DebugX.Log(DebugKey.Server, "The lobby was closed because there was no one left in the lobby. Lobby Code: " + lobbyVo.lobbyCode);

            DeleteLobby(lobbyVo.lobbyCode);
        }
        
        
        public Color ColorGenerator()
        {
            float r = Random.Range(0, 1f);
            float g = Random.Range(0, 1f);
            float b = Random.Range(0, 1f);

            Color color = new(r, g, b);
            return color;
        }

        public void OnAddBot(ushort fromId, string lobbyCode)
        {
            if (lobbies[lobbyCode].hostId == fromId)
            {
                UnityWebRequest.Get("http://localhost:8080/" + lobbyCode).SendWebRequest();
            }
        }
    }
}