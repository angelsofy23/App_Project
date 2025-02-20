using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Waypoint Status")]
    // Referência ao waypoint anterior e ao próximo 
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;

    [Range(0f, 5f)]  // Largura do waypoint - usada para determinar a área onde o carro pode estar
    public float waypointWidth = 5f;

    public Vector3 GetPosition()
    {
        // Calcula a posição mínima dentro da largura do waypoint, efetuando uma deslocação na direção do eixo X
        Vector3 minBound = transform.position + transform.right * waypointWidth / 2f;

        // Calcula a posição máxima dentro da largura do waypoint, efetuando uma deslocação na direção do eixo X
        Vector3 maxBound = transform.position + transform.right * waypointWidth / 2f;

        // Retorna um ponto aleatório entre a posição mínima e máxima, dentro da largura definida
        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
