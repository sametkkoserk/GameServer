using System;
using System.Collections.Generic;
using Runtime.Contexts.MainGame.Vo;
using UnityEngine;

namespace Runtime.Contexts.MainGame.ScriptableObjects
{
  [CreateAssetMenu(menuName = "Tools/PlayerAction/Create", fileName = "PlayerActionData")]
  [Serializable]
  public class PlayerActionData : ScriptableObject
  {
      public List<PlayerActionPermissionReferenceVo> playerActionNecessaryVos;
  }
}