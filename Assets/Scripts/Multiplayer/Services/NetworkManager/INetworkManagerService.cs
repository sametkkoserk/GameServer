namespace Multiplayer.Services.NetworkManager
{
    public interface INetworkManagerService
    {
        void Connect(ushort _port ,ushort _maxClientCount);
        void Ticker();
        void OnQuit();
    }
}
