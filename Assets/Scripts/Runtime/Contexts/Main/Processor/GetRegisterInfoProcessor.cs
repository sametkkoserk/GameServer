using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Main.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.examples.multiplecontexts.main;
using strange.extensions.command.impl;
using MainEvent = Runtime.Contexts.Main.Enum.MainEvent;

namespace Runtime.Contexts.Main.Processor
{
  public class GetRegisterInfoProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;
      ushort fromId = messageReceivedVo.fromId;
      byte[] message = messageReceivedVo.message;
      
      RegisterInfoVo registerInfoVo = networkManager.GetData<RegisterInfoVo>(message);

      RegisterInfoVo vo = new()
      {
        userId = fromId,
        username = registerInfoVo.username
      };
      
      dispatcher.Dispatch(MainEvent.CheckUserInfo,vo);
    }
  }
}