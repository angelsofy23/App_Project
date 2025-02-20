using UnityEngine;

public class EnemyCarWaypoints : MonoBehaviour
{
    [Header("Enemy Car")]
    public EnemyCar enemyCar;
    // Waypoint atual em que o carro inimigo está
    public Waypoint currentWaypoint;

    void Start()
    {
        // No início, indica ao carro inimigo que se mova até o primeiro waypoint
        enemyCar.LocateDestination(currentWaypoint.GetPosition());
    }

    void Update()
    {
        if (enemyCar.destinationReached) // Se o carro alcançou o waypoint atual
        {
            currentWaypoint = currentWaypoint.nextWaypoint;  // Atualiza o waypoint para o próximo da lista
            enemyCar.LocateDestination(currentWaypoint.GetPosition());   // Indica ao carro que se mova até o novo waypoint
        }
    }
}
