using Editor.Tools.DebugX.Runtime;
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
      PlayerReadyVo playerReadyVo = (PlayerReadyVo)evt.data;

      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.PlayerReadyResponse);
      message = networkManager.SetData(message, playerReadyVo);

      // message.AddUShort(playerReadyResponseVo.inLobbyId);
      // message.AddBool(playerReadyResponseVo.startGame);
      networkManager.SendToLobby(message, playerReadyVo.clients);

      if (playerReadyVo.startGame)
      {
        crossDispatcher.Dispatch(MainGameEvent.CreateMap);

      }
      
      DebugX.Log(DebugKey.Request, $" Player's Lobby ID: {playerReadyVo.id}, Lobby ID: {playerReadyVo.lobbyCode}, Process: Player Ready");
    }
  }
}