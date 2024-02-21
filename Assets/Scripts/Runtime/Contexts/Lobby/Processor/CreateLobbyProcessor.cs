using System.Collections.Generic;
using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Lobby.Processor
{
    public class CreateLobbyProcessor : EventCommand
    {
        [Inject] 
        public ILobbyModel lobbyModel { get; set; }
        [Inject] 
        public IPlayerModel playerModel { get; set; }
        [Inject]
        public INetworkManagerService networkManager { get; set; }

        public override void Execute()
        {
            MessageReceivedVo vo = (MessageReceivedVo)evt.data;
            ushort fromId = vo.fromId;
            byte[] message = vo.message;
            
            LobbyVo lobbyVo = networkManager.GetData<LobbyVo>(message);
            lobbyVo.hostId = fromId;
            lobbyVo.clients = new Dictionary<ushort, ClientVo>();
            DebugX.Log(DebugKey.Response, "Create Lobby message received.");
            
            lobbyModel.NewLobbyCreated(lobbyVo);
            ClientVo clientVo = new()
            {
                id = fromId,
                playerColor = new PlayerColorVo(lobbyModel.ColorGenerator()),
                userName = playerModel.userList[fromId].username
            };
            lobbyModel.OnJoin(lobbyVo.lobbyCode,clientVo);
        }
    }
}