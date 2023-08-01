using ProtoBuf;
using UnityEngine;

namespace Runtime.Contexts.Network.Vo
{
  [ProtoContract]

  public class PlayerColorVo
  {
    [ProtoMember(1)]
    public float r;

    [ProtoMember(2)]
    public float g;

    [ProtoMember(3)]
    public float b;
    
    public PlayerColorVo(){}
    
    public PlayerColorVo(Color color)
    {
      r = color.r;
      g = color.g;
      b = color.b;
    }
    
    public Color ToColor()
    {
      return new Color(r, g, b);
    }
  }
}