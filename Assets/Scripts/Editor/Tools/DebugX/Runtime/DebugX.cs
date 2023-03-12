using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace Editor.Tools.DebugX.Runtime
{
  [CreateAssetMenu(menuName = "Tools/Debug X/Create Debug X Data", fileName = "DebugXData")]
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
#if UNITY_EDITOR
      LogMap = new Dictionary<DebugKey, LoggerVo>();

      DebugX data = AssetDatabase.LoadAssetAtPath<DebugX>(DataPath);

      foreach (LoggerVo vo in data.loggerList)
      {
        LogMap[vo.key] = vo;
      }

      _inited = true;
#endif
    }

    /// <summary>Logs a message.</summary>
    /// <param name="tag">The tag of message.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="logKey">The log type of message. <seealso cref="LogKey"/></param>
    public static void Log(DebugKey tag, string message, LogKey logKey = LogKey.Log)
    {
#if UNITY_EDITOR
      if (!_inited) Init();

      if (!LogMap.ContainsKey(tag)) return;

      LoggerVo loggerVo = LogMap[tag];

      if (!loggerVo.active) return;

      string text = string.Format("<color=#{0}>" + "<b>{1}</b>" + "</color>: {2}", ColorUtility.ToHtmlStringRGBA(loggerVo.color), tag, message);

      switch (logKey)
      {
        case LogKey.Log:
          Debug.Log($"[{Thread.CurrentThread.ManagedThreadId}] {text}");
          return;
        case LogKey.Warning:
          Debug.LogWarning($"[{Thread.CurrentThread.ManagedThreadId}] {text}");
          return;
        case LogKey.Error:
          Debug.LogError($"[{Thread.CurrentThread.ManagedThreadId}] {text}");
          return;
      }
#endif
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Debug X/Create Debug X Data")]
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

    [MenuItem("Tools/Debug X/Activate All Logger")]
    public static void ActivateLoggers()
    {
      DebugX data = AssetDatabase.LoadAssetAtPath<DebugX>(DataPath);

      for (int i = 0; i < data.loggerList.Count; i++)
      {
        data.loggerList[i].active = true;
      }
      
      AssetDatabase.SaveAssets();
    }
    
    [MenuItem("Tools/Debug X/Deactivate All Logger")]
    public static void DeactivateLoggers()
    {
      DebugX data = AssetDatabase.LoadAssetAtPath<DebugX>(DataPath);

      for (int i = 0; i < data.loggerList.Count; i++)
      {
        data.loggerList[i].active = false;
      }
      
      AssetDatabase.SaveAssets();
    }
#endif
  }
}