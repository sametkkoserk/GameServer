using System.Collections.Generic;
using Lobby.Enum;
using Lobby.Model.LobbyModel;
using Lobby.Vo;
using Network.Enum;
using Network.Services.NetworkManager;
using Network.Vo;
using Riptide;
using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Lobby.Processor
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