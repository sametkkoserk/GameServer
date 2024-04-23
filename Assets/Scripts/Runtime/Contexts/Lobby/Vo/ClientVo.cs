using ProtoBuf;
using Runtime.Contexts.Network.Vo;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class ClientVo
  {
    [ProtoMember(1)]
    public ushort id;

    [ProtoMember(2)]
    public string userName;

    [ProtoMember(3)]
    public PlayerColorVo playerColor;
    
    [ProtoMember(4)]
    public ushort state;
  }

  public enum ClientState
  {
    None,
    LobbyReady,
    MainGameSceneReady,
    MainGameStart,
    MiniGameSceneReady,
    MiniGameCreated
  }
}