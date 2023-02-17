using System.Collections.Generic;
using Lobby.Model.LobbyModel;
using Lobby.Vo;
using Network.Enum;
using Network.Services.NetworkManager;
using Riptide;
using strange.extensions.command.impl;

namespace Lobby.Command
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
          message.AddUShort(lobbyVo.leaderId);
        }
      }
      networkManager.Server.Send(message,clientId);
    }
  }
}