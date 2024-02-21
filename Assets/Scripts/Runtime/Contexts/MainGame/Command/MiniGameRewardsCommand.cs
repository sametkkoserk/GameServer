using System.Collections.Generic;
using System.Linq;
using Riptide;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Command
{
  public class MiniGameRewardsCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      List<MiniGameStatsVo> miniGameStatsVos = (List<MiniGameStatsVo>)evt.data;

      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.MiniGameRewards);

      message = networkManager.SetData(message, miniGameStatsVos);
      networkManager.SendToLobby(message, miniGameStatsVos.ElementAt(0).clients);
    }
  }
}