using RiptideNetworking;

namespace Multiplayer.Services.NetworkManager
{
    public interface INetworkManagerService
    {
        Server Server { get; }
        
        void StartServer(ushort _port ,ushort _maxClientCount);
        void Ticker();
        void OnQuit();
    }
}
