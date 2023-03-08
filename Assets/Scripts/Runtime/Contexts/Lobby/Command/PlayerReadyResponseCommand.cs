using Riptide;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Runtime.Contexts.Lobby.Command
{
  public class PlayerReadyResponseCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }

    public override void Execute()
    {
      PlayerReadyResponseVo playerReadyResponseVo = (PlayerReadyResponseVo)evt.data;

      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.PlayerReadyResponse);
      message = networkManager.SetData(message, playerReadyResponseVo);

      message.AddUShort(playerReadyResponseVo.inLobbyId);
      message.AddBool(playerReadyResponseVo.startGame);

      for (ushort i = 0; i < playerReadyResponseVo.lobbyVo.clients.Count; i++)
      {
        networkManager.Server.Send(message, playerReadyResponseVo.lobbyVo.clients[i].id);
      }

      crossDispatcher.Dispatch(MainGameEvent.CreateMap, playerReadyResponseVo.lobbyVo);
    }
  }
}