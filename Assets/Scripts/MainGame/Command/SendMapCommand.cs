using System.Collections.Generic;
using MainGame.Vo;
using Network.Enum;
using Network.Services.NetworkManager;
using Riptide;
using strange.extensions.command.impl;

namespace MainGame.Command
{
  public class SendMapCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      MapGeneratorVo mapGeneratorVo = (MapGeneratorVo) evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort) ServerToClientId.SendMap);

      message.AddInt(mapGeneratorVo.cityVos.Count);

      foreach(KeyValuePair<int, CityVo> entry in mapGeneratorVo.cityVos)
      {
        CityVo cityVo = entry.Value;

        message.AddInt(cityVo.ID);
        message.AddBool(cityVo.isPlayable);
        message.AddInt(cityVo.soldierCount);
        message.AddVector3(cityVo.position);
        message.AddInt(cityVo.ownerID);
      }
      
      for (int i = 0; i < mapGeneratorVo.clients.Count; i++)
      {
        networkManager.Server.Send(message, mapGeneratorVo.clients[i].id);
      }
    }
  }
}