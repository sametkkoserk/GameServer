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
        Vector3 nextPoint=playerSpawnPoints[counter].position;
        counter++;
        return nextPoint;
    }
}
