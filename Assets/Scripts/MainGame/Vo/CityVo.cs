using System.Collections.Generic;
using UnityEngine;

namespace MainGame.Vo
{
  public class CityVo
  {
    public int ID;
    
    public bool isPlayable;

    public ushort soldierCount;

    public Vector3 position;

    public List<ushort> neighbors;

    public int ownerID;
  }
}