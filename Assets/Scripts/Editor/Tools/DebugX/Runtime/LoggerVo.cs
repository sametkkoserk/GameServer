using System;
using UnityEngine;

namespace Editor.Tools.DebugX.Runtime
{
  [Serializable]
  public class LoggerVo
  {
    public DebugKey key;

    public bool active;

    public Color color = Color.white;
  }
}