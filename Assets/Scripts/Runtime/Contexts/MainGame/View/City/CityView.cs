using System.Collections.Generic;
using strange.extensions.mediation.impl;

namespace Runtime.Contexts.MainGame.View.City
{
  public class CityView : EventView
  {
    private int id;

    private List<int> neighbors;

    private string lobbyCode; 

    public void Init(int cityId, string cityLobbyCode)
    {
      id = cityId;
      lobbyCode = cityLobbyCode;
    }

    public int GetId()
    {
      return id;
    }

    public void SetNeighbors(int neighborId)
    {
      neighbors.Add(neighborId);
    }

    public List<int> GetNeighbors()
    {
      return neighbors;
    }

    public string GetLobbyCode()
    {
      return lobbyCode;
    }
  }
}