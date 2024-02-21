using Riptide;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MiniGames.Command
{
    public class MiniGameEndedCommand : EventCommand
    {
        [Inject] public INetworkManagerService networkManager { get; set; }

        public override void Execute()
        {
            InfoVo vo = (InfoVo)evt.data;

            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.MiniGaneEnded);
            networkManager.SendToLobby(message,vo.clients);
        }

    }
}