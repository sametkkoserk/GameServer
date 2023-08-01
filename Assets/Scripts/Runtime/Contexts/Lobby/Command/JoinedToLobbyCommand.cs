using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Command
{
  public class JoinedToLobbyCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      JoinedToLobbyVo vo = (JoinedToLobbyVo)evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.JoinedToLobby);
      message = networkManager.SetData(message, vo);
      networkManager.Server.Send(message, vo.clientVo.id);

      Message messageToOthers = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.NewPlayerToLobby);
      messageToOthers = networkManager.SetData(messageToOthers, vo);
      
      DebugX.Log(DebugKey.JoinServer, 
        $"Player ID: {vo.clientVo.id}, Player's Lobby ID: {vo.clientVo.inLobbyId}, Lobby Code: {vo.lobbyVo.lobbyCode}");
      
      networkManager.SendToLobbyExcept(messageToOthers,vo.clientVo.inLobbyId, vo.lobbyVo.clients);
    }
  }
}