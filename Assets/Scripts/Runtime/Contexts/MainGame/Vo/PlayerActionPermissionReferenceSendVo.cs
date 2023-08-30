using System.Collections.Generic;
using ProtoBuf;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class PlayerActionPermissionReferenceSendVo
  {
    [ProtoMember(1)]
    public Dictionary<PlayerActionKey, PlayerActionPermissionReferenceVo> allPlayerActionsReferenceList { get; set; }

    [ProtoIgnore]
    public Dictionary<ushort, ClientVo> clients;
  }
}