using System.Collections.Generic;
using Riptide;
using Runtime.Contexts.Lobby.Vo;

namespace Runtime.Contexts.Network.Services.NetworkManager
{
    public interface INetworkManagerService
    {
        Server Server { get; }

        void StartServer(ushort _port ,ushort _maxClientCount);

        void SendToLobby(Message message, Dictionary<ushort,ClientVo> clients);
        void SendToLobbyExcept(Message message, ushort exceptClient, Dictionary<ushort, ClientVo> clients);
        
        T GetData<T>(string message) where T : new();
        Message SetData(Message message, object obj);
        void Ticker();
        void OnQuit();
    }
}
