using System.Collections.Generic;
using Runtime.Lobby.Vo;
using Runtime.MainGame.Vo;
using strange.extensions.mediation.impl;

namespace Runtime.MainGame.View.MainMap
{
  public class MainMapView : EventView
  {
    public Dictionary<int, CityVo> cities;

    public LobbyVo lobbyVo;
  }
}