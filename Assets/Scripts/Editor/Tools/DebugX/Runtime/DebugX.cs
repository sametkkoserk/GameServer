using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace Editor.Tools.DebugX.Runtime
{
  [CreateAssetMenu(menuName = "Project/Special/DebugX", fileName = "DebugXData")]
  [Serializable]
  public class DebugX : ScriptableObject
  {
    [NonSerialized]
    public const string DataPath = "Assets/Scripts/Editor/Tools/DebugX/ScriptableObject/DebugXData.asset";

    [Header("Attributes")]
    public List<LoggerVo> loggerList;

    private static Dictionary<DebugKey, LoggerVo> LogMap;

    private static bool _inited;

    public static void Init()
    {
      LogMap = new Dictionary<DebugKey, LoggerVo>();

      DebugX data = AssetDatabase.LoadAssetAtPath<DebugX>(DataPath);

      foreach (LoggerVo vo in data.loggerList)
      {
        LogMap[vo.key] = vo;
      }

      _inited = true;
    }

    /// <summary>Logs a message.</summary>
    /// <param name="tag">The tag of message.</param>
    /// <param name="message">The message to log.</param>
    public static void Log(DebugKey tag, string message)
    {
      if (!_inited) Init();

      if (!LogMap.ContainsKey(tag)) return;

      LoggerVo loggerVo = LogMap[tag];

      if (!loggerVo.active) return;

      string text = string.Format("<color=#{0}>" + "<b>{1}</b>" + "</color>: {2}", ColorUtility.ToHtmlStringRGBA(loggerVo.color), tag, message);

      Debug.Log($"[{Thread.CurrentThread.ManagedThreadId}] {text}");
    }

#if UNITY_EDITOR
    [
      MenuItem("Assets/Create/BrosData/Logger Settings")]
    public static void CreateMyAsset()
    {
      DebugX old = AssetDatabase.LoadAssetAtPath<DebugX>(DataPath);
      if (old != null)
      {
        Selection.activeObject = old;
        return;
      }

      DebugX asset = CreateInstance<DebugX>();
      AssetDatabase.CreateAsset(asset, DataPath);
      AssetDatabase.SaveAssets();
      EditorUtility.FocusProjectWindow();
      Selection.activeObject = asset;
    }
#endif
  }
}