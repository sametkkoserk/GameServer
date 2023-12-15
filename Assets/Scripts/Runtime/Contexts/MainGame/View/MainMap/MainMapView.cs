using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.View.MainGameManager;
using Runtime.Contexts.MainGame.Vo;
using strange.extensions.mediation.impl;

namespace Runtime.Contexts.MainGame.View.MainMap
{
  public class MainMapView : EventView
  {
    public Dictionary<int, CityVo> cities;
    
    public LobbyVo lobbyVo;

    public MainGameManagerMediator mainGameManagerMediator = new();
    
    private List<int> readyCities = new();

    public void SetCities(Dictionary<int, CityVo> cityVos)
    {
      cities = cityVos;
    }

    public Dictionary<int, CityVo> GetCities()
    {
      return cities;
    }

    public CityVo GetSpecificCity(int id)
    {
      return cities[id];
    }

    public void SetSpecificCity(CityVo cityVo)
    {
      cities[cityVo.ID] = cityVo;
    }

    public List<int> GetReadyCities()
    {
      return readyCities;
    }
  }
}