using UnityEngine;

public class EnemyCarWaypoints : MonoBehaviour
{
    [Header("Enemy Car")]
    public EnemyCar enemyCar;
    // Waypoint atual em que o carro inimigo est�
    public Waypoint currentWaypoint;

    void Start()
    {
        // No in�cio, indica ao carro inimigo que se mova at� o primeiro waypoint
        enemyCar.LocateDestination(currentWaypoint.GetPosition());
    }

    void Update()
    {
        if (enemyCar.destinationReached) // Se o carro alcan�ou o waypoint atual
        {
            currentWaypoint = currentWaypoint.nextWaypoint;  // Atualiza o waypoint para o pr�ximo da lista
            enemyCar.LocateDestination(currentWaypoint.GetPosition());   // Indica ao carro que se mova at� o novo waypoint
        }
    }
}
