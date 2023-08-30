using System.Linq;
using Riptide;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MainGame.Command
{
  public class ChangePlayerActionCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      PlayerActionVo playerActionVo = (PlayerActionVo)evt.data;
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.PlayerActionChanged);

      for (int i = 0; i < playerActionVo.playerActionKeys.Count; i++)
      {
        message = networkManager.SetData(message, playerActionVo.playerActionKeys.ElementAt(i).Value);
        networkManager.Server.Send(message, playerActionVo.playerActionKeys.ElementAt(i).Key);
      }
    }
  }
}