using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.MainGameManager
{
  public class MainGameManagerView : EventView
  {
    [HideInInspector]
    public List<ushort> queueList;

    [HideInInspector]
    public int queue;

    public TurnVo turnVo = new();
    
    public LobbyVo lobbyVo;
  }
}