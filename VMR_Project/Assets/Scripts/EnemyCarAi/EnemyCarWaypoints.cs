using UnityEngine;

public class EnemyCarWaypoints : MonoBehaviour
{
    [Header("Enemy Car")]
    public EnemyCar enemyCar;
    public Waypoint currentWaypoint;

    void Start()
    {
        enemyCar.LocateDestination(currentWaypoint.GetPosition());
    }

    void Update()
    {
        if (enemyCar.destinationReached)
        {
            currentWaypoint = currentWaypoint.nextWaypoint;
            enemyCar.LocateDestination(currentWaypoint.GetPosition());
        }
    }
}
