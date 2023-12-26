using System;
using System.Collections;
using System.Collections.Generic;
using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MiniGames.MiniGames.Race;
using UnityEngine;

public class FinishLineController : MonoBehaviour
{
    private RaceController raceController;
    private void Start()
    {
        raceController = GetComponentInParent<RaceController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("finishedTheGame");
        if (other.tag=="Player")
        {
            raceController.ClientFinished(other.GetComponent<CarController>().clientId);
        }
    }
}
