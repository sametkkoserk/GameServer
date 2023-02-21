using System.Collections.Generic;
using Riptide;
using Runtime.Lobby.Model.LobbyModel;
using Runtime.Lobby.Vo;
using Runtime.Network.Enum;
using Runtime.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Lobby.Command
{
  public class SendLobbiesCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }
    public override void Execute()
    {
      ushort clientId = (ushort)evt.data;
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.SendLobbies);
      Dictionary<ushort, LobbyVo> lobbyDict = lobbyModel.lobbies;
      LobbyVo lobbyVo;
      message.AddInt(lobbyDict.Count);
      for (ushort i = 0; i < 10; i++)
      {
        if (lobbyDict.ContainsKey(i))
        {
          lobbyVo = lobbyDict[i];
          message.AddUShort(lobbyVo.lobbyId);
          message.AddString(lobbyVo.lobbyName);
          message.AddBool(lobbyVo.isPrivate);
          message.AddUShort(lobbyVo.playerCount);
          message.AddUShort(lobbyVo.maxPlayerCount);
          message.AddUShort(lobbyVo.leaderId);
        }
      }
      networkManager.Server.Send(message,clientId);
    }
  }
}