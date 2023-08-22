using System.Linq;
using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model.MainGameModel;
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
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    public override void Execute()
    {
      PlayerReadyVo playerReadyVo = (PlayerReadyVo)evt.data;

      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.PlayerReadyResponse);
      message = networkManager.SetData(message, playerReadyVo);

      networkManager.SendToLobby(message, playerReadyVo.clients);

      if (playerReadyVo.startGame)
      {
        for (int i = 0; i < playerReadyVo.lobbyVo.playerCount; i++)
        {
          playerReadyVo.lobbyVo.clients.ElementAt(i).Value.ready = false;
        }
        playerReadyVo.lobbyVo.readyCount = 0;
        
        mainGameModel.mapLobbyVos.Add(playerReadyVo.lobbyVo);
        
        crossDispatcher.Dispatch(MainGameEvent.CreateMap);
      }
      
      DebugX.Log(DebugKey.Request, $" Player's Lobby ID: {playerReadyVo.id}, Lobby Code: {playerReadyVo.lobbyCode}, Process: Player Ready");
    }
  }
}