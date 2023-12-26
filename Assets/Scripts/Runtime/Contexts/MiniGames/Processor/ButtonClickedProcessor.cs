using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MiniGames.Model.MiniGamesModel;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MiniGames.Processor
{
    public class ButtonClickedProcessor : EventCommand
    {
        [Inject] public INetworkManagerService networkManager { get; set; }
        [Inject] public IMiniGamesModel miniGamesModel { get; set; }
        public override void Execute()
        {
            MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;
            ushort clientId = messageReceivedVo.fromId;

            ClickedButtonsVo vo = networkManager.GetData<ClickedButtonsVo>(messageReceivedVo.message);
            miniGamesModel.OnButtonClicked(clientId, vo);
            

            DebugX.Log(DebugKey.Processor, $"Received: ButtonClickedProcessor");
        }

    }
}