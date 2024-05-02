using UnityEngine;

namespace Runtime.Contexts.MiniGames.MiniGames.Race
{
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
            if (other.CompareTag("Player"))
            {
                raceController.ClientFinished(other.GetComponent<CarController>().clientId);
            }
        }
    }
}
