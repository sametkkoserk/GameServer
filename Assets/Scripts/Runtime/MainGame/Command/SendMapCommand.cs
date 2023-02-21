using System.Collections.Generic;
using Riptide;
using Runtime.MainGame.Vo;
using Runtime.Network.Enum;
using Runtime.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.MainGame.Command
{
  public class SendMapCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      MapGeneratorVo mapGeneratorVo = (MapGeneratorVo) evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort) ServerToClientId.SendMap);
      message=networkManager.SetData(message,mapGeneratorVo);

      for (ushort i = 0; i < mapGeneratorVo.clients.Count; i++)
      {
        networkManager.Server.Send(message, mapGeneratorVo.clients[i].id);
      }
    }
  }
}