using System.Collections.Generic;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Processor
{
    public class CreateLobbyProcessor : EventCommand
    {
        [Inject] 
        public ILobbyModel lobbyModel { get; set; }
        
        [Inject]
        public INetworkManagerService networkManager { get; set; }

        public override void Execute()
        {
            MessageReceivedVo vo = (MessageReceivedVo)evt.data;
            ushort fromId = vo.fromId;
            string message = vo.message;
            
            LobbyVo lobbyVo = networkManager.GetData<LobbyVo>(message);
            lobbyVo.leaderId = fromId;
            Debug.Log(message);
            Debug.Log(fromId);
            // lobbyVo.lobbyName = message.GetString();
            // lobbyVo.isPrivate = message.GetBool();
            // lobbyVo.maxPlayerCount = message.GetUShort();
            // lobbyVo.leaderId = fromId;
            // lobbyVo.playerCount = 0;
            lobbyVo.clients = new Dictionary<ushort, ClientVo>();
            Debug.Log("CreateLobby message received");
            lobbyModel.NewLobbyCreated(lobbyVo);
            dispatcher.Dispatch(LobbyEvent.CreateLobby);
            
            
        }
    }
}