using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine.Networking;

namespace Runtime.Contexts.Lobby.Processor
{
  public class AddBotProcessor : EventCommand
  {
    [Inject] 
    public ILobbyModel lobbyModel { get; set; }
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    public override void Execute()
    {
      
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      ushort fromId = vo.fromId;
      byte[] message = vo.message;
      string lobbyCode = networkManager.GetData<string>(message);

      lobbyModel.OnAddBot(fromId,lobbyCode);
    }
  }
}