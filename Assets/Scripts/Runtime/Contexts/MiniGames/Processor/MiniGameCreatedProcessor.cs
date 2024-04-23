using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MiniGames.Model.MiniGamesModel;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MiniGames.Processor
{
    public class MiniGameCreatedProcessor : EventCommand
    {
        [Inject] public INetworkManagerService networkManager { get; set; }

        [Inject] public IMiniGamesModel miniGamesModel { get; set; }
        public override void Execute()
        {
            MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;
            ushort clientId = messageReceivedVo.fromId;

            ReadyVo vo = networkManager.GetData<ReadyVo>(messageReceivedVo.message);
            miniGamesModel.OnMiniGameCreated(vo.lobbyCode,clientId);
        }

    }
}