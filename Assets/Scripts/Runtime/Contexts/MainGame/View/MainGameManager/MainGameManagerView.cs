using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Contexts.MainGame.View.MainGameManager
{
  public class MainGameManagerView : EventView
  {
    [HideInInspector]
    public ushort readyCount;

    [FormerlySerializedAs("randomNumbers")]
    [HideInInspector]
    public List<int> queueList;

    [HideInInspector]
    public ushort queue;
    
    public LobbyVo lobbyVo;
    
    
  }
}