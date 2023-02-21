using Riptide;

namespace Runtime.Network.Services.NetworkManager
{
    public interface INetworkManagerService
    {
        Server Server { get; }

        void StartServer(ushort _port ,ushort _maxClientCount);

        T GetData<T>(string message) where T : new();
        Message SetData(Message message, object obj);
        void Ticker();
        void OnQuit();
    }
}
