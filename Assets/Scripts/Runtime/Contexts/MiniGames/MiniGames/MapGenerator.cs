using Runtime.Contexts.MiniGames.Vo;
using UnityEngine;

namespace Runtime.Contexts.MiniGames.MiniGames
{
    public abstract class MapGenerator : MonoBehaviour
    {
        public virtual MiniGameMapGenerationVo SetMap()
        {
            return null;
        }
    }
}