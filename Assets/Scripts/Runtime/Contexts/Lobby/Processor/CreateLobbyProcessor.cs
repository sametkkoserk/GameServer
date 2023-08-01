using System.Collections.Generic;
using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

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
            byte[] message = vo.message;
            
            LobbyVo lobbyVo = networkManager.GetData<LobbyVo>(message);
            lobbyVo.leaderId = fromId;
            lobbyVo.clients = new Dictionary<ushort, ClientVo>();
            DebugX.Log(DebugKey.Response, "Create Lobby message received.");
            
            lobbyModel.NewLobbyCreated(lobbyVo);
            dispatcher.Dispatch(LobbyEvent.CreateLobby);
        }
    }
}