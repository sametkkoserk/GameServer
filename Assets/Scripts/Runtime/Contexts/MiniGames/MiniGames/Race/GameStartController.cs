using System.Collections;
using System.Collections.Generic;
using Runtime.Contexts.MiniGames.MiniGames.Race;
using UnityEngine;

public class GameStartController : MonoBehaviour
{
    public List<Transform> playerSpawnPoints;
    private int counter;
    
    public Vector3 GetNextPoint()
    {
        return playerSpawnPoints[counter].position;
        counter++;
    }
}
