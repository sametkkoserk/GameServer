using Riptide;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MiniGames.Command
{
    public class SendStateCommand : EventCommand
    {
        [Inject] public INetworkManagerService networkManager { get; set; }

        public override void Execute()
        {
            SendPacketToLobbyVo<MiniGameStateVo> vo = (SendPacketToLobbyVo<MiniGameStateVo>)evt.data;

            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.SendMiniGameState);
            message = networkManager.SetData(message, vo.mainClass);
            networkManager.SendToLobby(message,vo.clients);
        }

    }
}