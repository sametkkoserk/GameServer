using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.Main.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Modules.Core.Discord.Enum;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Runtime.Contexts.Main.Command
{
  public class CheckUserInfoCommand : EventCommand
  {
    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }
    
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void Execute()
    {
      RegisterInfoVo registerInfoVo = (RegisterInfoVo)evt.data;
      //if (smt)
      // There will be checking system in the future.

      playerModel.userList.Add(registerInfoVo.userId, registerInfoVo);
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.RegisterAccepted);

      message = networkManager.SetData(message, registerInfoVo);

      networkManager.Server.Send(message, registerInfoVo.userId);
      
      crossDispatcher.Dispatch(DiscordEvent.NewPlayerJoined, registerInfoVo.username);
      DebugX.Log(DebugKey.Request, "Register accepted. New player joined!");
    }
  }
}