using System.Collections.Generic;
using Riptide;
using Runtime.Lobby.Enum;
using Runtime.Lobby.Model.LobbyModel;
using Runtime.Lobby.Vo;
using Runtime.Network.Vo;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Lobby.Processor
{
    public class CreateLobbyProcessor : EventCommand
    {
        [Inject] 
        public ILobbyModel lobbyModel { get; set; }
        public override void Execute()
        {
            MessageReceivedVo vo = (MessageReceivedVo)evt.data;
            ushort fromId = vo.fromId;
            Message message = vo.message;

            LobbyVo lobbyVo = new LobbyVo();
            lobbyVo.lobbyName = message.GetString();
            lobbyVo.isPrivate = message.GetBool();
            lobbyVo.maxPlayerCount = message.GetUShort();
            lobbyVo.leaderId = fromId;
            lobbyVo.playerCount = 0;
            lobbyVo.clients = new Dictionary<ushort, ClientVo>();
            Debug.Log("CreateLobby message received");
            lobbyModel.NewLobbyCreated(lobbyVo);
            dispatcher.Dispatch(LobbyEvent.CreateLobby);
            
            
        }
    }
}