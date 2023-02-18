using MainGame.Enum;
using Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace MainGame.Command
{
  public class CreateMapCommand : EventCommand
  {
    public override void Execute()
    {
      dispatcher.Dispatch(MainGameEvent.CreateMap);
    }
  }
}