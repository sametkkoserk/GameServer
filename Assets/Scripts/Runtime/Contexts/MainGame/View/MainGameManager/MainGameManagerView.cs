using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.MainGameManager
{
  public class MainGameManagerView : EventView
  {
    [HideInInspector]
    public GameManagerVo gameManagerVo = new();

    [HideInInspector]
    public LobbyVo lobbyVo;
  }
}