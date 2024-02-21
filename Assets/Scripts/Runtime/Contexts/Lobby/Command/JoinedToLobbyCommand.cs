using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Lobby.Command
{
  public class JoinedToLobbyCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void Execute()
    {
      JoinedToLobbyVo vo = (JoinedToLobbyVo)evt.data;
      playerModel.userList[vo.clientVo.id].lobbyCode = vo.lobbyVo.lobbyCode;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.JoinedToLobby);
      message = networkManager.SetData(message, vo);
      networkManager.Server.Send(message, vo.clientVo.id);

      Message messageToOthers = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.NewPlayerToLobby);
      messageToOthers = networkManager.SetData(messageToOthers, vo);
      
      DebugX.Log(DebugKey.JoinServer, 
        $"Player ID: {vo.clientVo.id}, Lobby Code: {vo.lobbyVo.lobbyCode}");
      
      networkManager.SendToLobbyExcept(messageToOthers,vo.clientVo.id, vo.lobbyVo.clients);
    }
  }
}