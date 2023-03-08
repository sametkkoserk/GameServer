using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.MainGameManager
{
  public class MainGameManagerView : EventView
  {
    [HideInInspector]
    public ushort readyCount;

    [HideInInspector]
    public List<int> randomNumbers;

    [HideInInspector]
    public ushort queue;
    
    public LobbyVo lobbyVo;
    
    
  }
}