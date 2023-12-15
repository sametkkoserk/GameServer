using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.View.MainMap;
using Runtime.Contexts.MainGame.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.MainGameManager
{
  public class MainGameManagerView : EventView
  {
    public GameManagerVo gameManagerVo = new();

    public MainMapMediator mainMapMediator = new();

    [HideInInspector]
    public LobbyVo lobbyVo;
  }
}