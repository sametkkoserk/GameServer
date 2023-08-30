using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Vo;
using strange.extensions.mediation.impl;

namespace Runtime.Contexts.MainGame.View.MainMap
{
  public class MainMapView : EventView
  {
    public Dictionary<int, CityVo> cities;
    
    public LobbyVo lobbyVo;
  }
}