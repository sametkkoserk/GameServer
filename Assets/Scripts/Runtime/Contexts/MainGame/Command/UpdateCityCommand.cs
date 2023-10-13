using Riptide;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MainGame.Command
{
  public class UpdateCityCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      SendPacketToLobbyVo<CityVo> cityVo = (SendPacketToLobbyVo<CityVo>)evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.UpdateCity);
      message = networkManager.SetData(message, cityVo.mainClass);
      
      networkManager.SendToLobby(message, cityVo.clients);
    }
  }
}