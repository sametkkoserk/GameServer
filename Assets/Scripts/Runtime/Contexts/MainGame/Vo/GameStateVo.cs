using ProtoBuf;
using Runtime.Contexts.MainGame.Enum;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class GameStateVo
  {
    [ProtoMember(1)]
    public GameStateKey gameStateKey;
  }
}