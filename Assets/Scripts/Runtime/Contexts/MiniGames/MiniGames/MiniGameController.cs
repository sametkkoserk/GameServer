using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MiniGames.View.MiniGame;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Vo;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Contexts.MiniGames.MiniGames
{
    public abstract class MiniGameController : MonoBehaviour
    {
        public MiniGameMediator miniGameMediator;
        public MiniGameKeyScriptable keys;
        public LobbyVo lobbyVo;
        public Transform playerContainer;

        public List<ushort> leaderBoard = new List<ushort>();
        
        
        public Dictionary<string, GameObject> sentObjs=new Dictionary<string, GameObject>();
        public Dictionary<string, GameObject> objs=new Dictionary<string, GameObject>();
        public Dictionary<string, GameObject> newObjs=new Dictionary<string, GameObject>();
        public Dictionary<ushort, GameObject> players=new Dictionary<ushort, GameObject>();
        private SendPacketToLobbyVo<MiniGameStateVo> stateVo = new SendPacketToLobbyVo<MiniGameStateVo>();

        public virtual void Start()
        {
            CreatePlayers();
        }

        public virtual void OnButtonClick(ushort clientId, ClickedButtonsVo vo)
        {
            
        }

        private void Update()
        {
            SetStateVo();
            miniGameMediator.SendState(stateVo);
        }

        private void SetStateVo()
        {
            stateVo.clients = lobbyVo.clients;
            stateVo.mainClass = new MiniGameStateVo()
            {
                objPositions = objs.ToDictionary(pair => pair.Key, pair => new Vector3Vo(pair.Value.transform.position)),
                objRotations = objs.ToDictionary(pair => pair.Key, pair => new QuaternionVo(pair.Value.transform.rotation)),

                newPositions = newObjs.ToDictionary(pair => pair.Key, pair => new Vector3Vo(pair.Value.transform.position)),
                newRotations = newObjs.ToDictionary(pair => pair.Key, pair => new QuaternionVo(pair.Value.transform.rotation)),

                playerPositions = players.ToDictionary(pair => pair.Key, pair => new Vector3Vo(pair.Value.transform.position)),
                playerRotations = players.ToDictionary(pair => pair.Key, pair => new QuaternionVo(pair.Value.transform.rotation)),
            };
            
        }
        
        public void ClientFinished(ushort clientId)
        {
            leaderBoard.Add(clientId);
            if (leaderBoard.Count==players.Count)
            {
                miniGameMediator.EndTheGame(leaderBoard);
            }
        }


        public virtual void CreateObj(string key,Transform transform)
        {
            Addressables.InstantiateAsync(key, transform).Completed+= handle =>
            {
                if (handle.Status==AsyncOperationStatus.Succeeded)
                {
                    GameObject obj = handle.Result;
                    newObjs[key] = obj;
                }
            } ;
        }
        
        public virtual void CreatePlayers()
        {
            for (int i = 0; i < lobbyVo.clients.Count; i++)
            {
                int index = i;
                Addressables.InstantiateAsync(keys.player, playerContainer).Completed+= handle =>
                {
                    if (handle.Status==AsyncOperationStatus.Succeeded)
                    {
                        GameObject obj = handle.Result;
                        players[lobbyVo.clients.ElementAt(index).Value.id] = obj;

                    }
                } ;
            }
        }
    }
}