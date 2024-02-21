using System.Linq;
using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Lobby.Command
{
  public class PlayerReadyResponseCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      PlayerReadyVo playerReadyVo = (PlayerReadyVo)evt.data;

      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.PlayerReadyResponse);
      message = networkManager.SetData(message, playerReadyVo);

      networkManager.SendToLobby(message, playerReadyVo.clients);

      if (playerReadyVo.startGame)
      {
        LobbyVo lobbyVo = lobbyModel.lobbies[playerReadyVo.lobbyCode];
        for (int i = 0; i < lobbyVo.playerCount; i++)
        {
          lobbyVo.clients.ElementAt(i).Value.ready = false;
        }
        lobbyVo.readyCount = 0;
        
        mainGameModel.mapLobbyVos.Add(lobbyVo);
        lobbyModel.lobbies.Remove(lobbyVo.lobbyCode);
        crossDispatcher.Dispatch(MainGameEvent.CreateMap, playerReadyVo.lobbyCode);
      }
      
      DebugX.Log(DebugKey.Request, $" Lobby Code: {playerReadyVo.lobbyCode}, Process: Player Ready");
    }
  }
}