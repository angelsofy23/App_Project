using UnityEngine;

public class EnemyCarWaypoints : MonoBehaviour
{
    [Header("Enemy Car")]
    public EnemyCar enemyCar;
    public Waypoint currentWaypoint;

    void Start()
    {
        enemyCar.LocateDestination(currentWaypoint.GetPosition());
        enemyCar.UpdateLastWaypoint(currentWaypoint);  // Atualiza o �ltimo waypoint logo no in�cio
    }

    void Update()
    {
        if (enemyCar.destinationReached)
        {
            currentWaypoint = currentWaypoint.nextWaypoint;
            enemyCar.LocateDestination(currentWaypoint.GetPosition());
            enemyCar.UpdateLastWaypoint(currentWaypoint);  // Atualiza o �ltimo waypoint ao alcan�ar um novo
        }
    }
}
