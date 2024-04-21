using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MiniGames.Command
{
    public class SendCreateMiniGameSceneCommand : EventCommand
    {
        [Inject] public INetworkManagerService networkManager { get; set; }

        public override void Execute()
        {
            SendPacketToLobbyVo<LobbyVo> vo = (SendPacketToLobbyVo<LobbyVo>)evt.data;

            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.CreateMiniGameScene);
            message = networkManager.SetData(message, vo.mainClass);
            DebugX.Log(DebugKey.Server,"SendCreateMiniGameSceneCommand");
            networkManager.SendToLobby(message,vo.clients);
        }

    }
}