using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FinishSystem : MonoBehaviour
{
    public int maxLaps;
    private int currentLap;

    private void OnTriggerEnter(Collider other)
    {
        EnemyCar enemyCar = other.GetComponent<EnemyCar>();
        CarController playerCar = other.GetComponent<CarController>();

        if (enemyCar != null)
        {
            enemyCar.IncreaseLap();
            CheckRaceCompletion(enemyCar);
        }

        if (playerCar != null)
        {
            playerCar.IncreaseLap();
            CheckRaceCompletion(playerCar);
        }
    }

    private void CheckRaceCompletion(EnemyCar enemyCar)
    {
        if (enemyCar.currentLap == maxLaps)
        {
            EndMission(false);
        }
    }

    private void CheckRaceCompletion(CarController playerCar)
    {
        if (playerCar.currentLap == maxLaps)
        {
            EndMission(true);
        }
    }

    void EndMission(bool success)
    {
        if (success)
        {
            Debug.Log("Player Wins");
        }
        else
        {
            Debug.Log("Player Lose");
        }
    }
}
