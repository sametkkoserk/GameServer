using System.Collections.Generic;
using Lobby.Vo;
using MainGame.Vo;
using strange.extensions.mediation.impl;

namespace MainGame.View.MainMap
{
  public class MainMapView : EventView
  {
    public Dictionary<int, CityVo> cities;

    public LobbyVo lobbyVo;
  }
}