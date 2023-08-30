using System;
using System.Collections.Generic;
using ProtoBuf;
using Runtime.Contexts.MainGame.Enum;
using UnityEngine;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  [Serializable]
  public class PlayerActionPermissionReferenceVo
  {
    [ProtoIgnore]
    public string name;
    
    [ProtoIgnore]
    public PlayerActionKey playerActionKey;
    
    [Space] [Space]
    [Tooltip("If the StateKey is present in this list, it is sufficient for it to function. It doesn't need to cover all of them.")]
    [ProtoMember(1)]
    public List<GameStateKey> gameStateKeys = new();

    [Space] [Space]
    [Tooltip("All ActionKeys must be present on the player.")]
    [ProtoMember(2)]
    public List<PlayerActionKey> playerActionNecessaryKeys = new();
  }
}