using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FinishSystem : MonoBehaviour
{
    public int maxLaps;
    private int currentLap;
    public GameObject winPanel;

    private void OnTriggerEnter(Collider other)
    {
        EnemyCar enemyCar = other.GetComponent<EnemyCar>();

        if (enemyCar != null)
        {
            enemyCar.IncreaseLap();
            CheckRaceCompletion(enemyCar);
        }

    }

    private void CheckRaceCompletion(EnemyCar enemyCar)
    {
        if (enemyCar.currentLap == maxLaps)
        {
            EndMission(false);
        }
    }

    public void EndMission(bool success)
    {
        if (success)
        {
            Debug.Log("Player Wins");
            winPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Player Lose");
        }
    }
}
