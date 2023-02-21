using Riptide;
using Runtime.Lobby.Vo;
using Runtime.MainGame.Enum;
using Runtime.Network.Enum;
using Runtime.Network.Services.NetworkManager;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Runtime.Lobby.Command
{
  public class PlayerReadyResponseCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set;}

    public override void Execute()
    {
      PlayerReadyVo playerReadyVo = (PlayerReadyVo)evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.PlayerReadyResponse);
      
      message.AddUShort(playerReadyVo.inLobbyId);
      message.AddBool(playerReadyVo.startGame);
      
      for (ushort i = 0; i < playerReadyVo.lobbyVo.clients.Count; i++)
      {
        networkManager.Server.Send(message,playerReadyVo.lobbyVo.clients[i].id);
      }

      crossDispatcher.Dispatch(MainGameEvent.CreateMap, playerReadyVo.lobbyVo);
      
    }
  }
}