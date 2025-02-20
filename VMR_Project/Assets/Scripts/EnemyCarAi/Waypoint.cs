using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Waypoint Status")]
    // Refer�ncia ao waypoint anterior e ao pr�ximo 
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;

    [Range(0f, 5f)]  // Largura do waypoint - usada para determinar a �rea onde o carro pode estar
    public float waypointWidth = 5f;

    public Vector3 GetPosition()
    {
        // Calcula a posi��o m�nima dentro da largura do waypoint, efetuando uma desloca��o na dire��o do eixo X
        Vector3 minBound = transform.position + transform.right * waypointWidth / 2f;

        // Calcula a posi��o m�xima dentro da largura do waypoint, efetuando uma desloca��o na dire��o do eixo X
        Vector3 maxBound = transform.position + transform.right * waypointWidth / 2f;

        // Retorna um ponto aleat�rio entre a posi��o m�nima e m�xima, dentro da largura definida
        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
