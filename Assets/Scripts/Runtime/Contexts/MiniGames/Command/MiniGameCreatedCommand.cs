using Riptide;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MiniGames.Command
{
    public class MiniGameCreatedCommand : EventCommand
    {
        [Inject] public INetworkManagerService networkManager { get; set; }

        public override void Execute()
        {
            SendPacketToLobbyVo<MiniGameCreatedVo> vo = (SendPacketToLobbyVo<MiniGameCreatedVo>)evt.data;

            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.MiniGameCreated);
            message = networkManager.SetData(message, vo.mainClass);
            
            networkManager.SendToLobby(message,vo.clients);
        }

    }
}