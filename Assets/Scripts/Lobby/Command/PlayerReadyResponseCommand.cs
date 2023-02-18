using Lobby.Vo;
using MainGame.Enum;
using Network.Enum;
using Network.Services.NetworkManager;
using Riptide;
using strange.examples.multiplecontexts.main;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Lobby.Command
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
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.playerReadyResponse);
      
      message.AddUShort(playerReadyVo.inLobbyId);
      message.AddBool(playerReadyVo.startGame);
      
      for (int i = 0; i < playerReadyVo.lobbyVo.clients.Count; i++)
      {
        networkManager.Server.Send(message,playerReadyVo.lobbyVo.clients[i].id);
      }

      crossDispatcher.Dispatch(MainGameEvent.CreateMap, playerReadyVo.lobbyVo);
    }
  }
}