using UnityEngine;
using System.Collections;

public class EnemyCar : MonoBehaviour
{
    [Header("Car Engine")]
    public float maxSpeed = 10f;
    public float currentSpeed;
    public float acceleration = 1f;
    public float turningSpeed = 30f;
    public float breakSpeed = 2f;

    [Header("Destination Var")]
    public Vector3 destination;
    public bool destinationReached;

    [Header("Lap")]
    public int maxLaps;
    public int currentLap;

    private Rigidbody rb;
    public bool isInactive = false;  // Flag para verificar se o inimigo está inativo
    private float deactivateTime;     // Tempo que o inimigo foi desativado
    private float deactivateDuration = 10f;  // Duração que o inimigo ficará inativo

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        // Adicionando restrições para evitar que o carro vire para cima
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        maxLaps = FindObjectOfType<FinishSystem>().maxLaps;
    }

    void FixedUpdate()
    {
        if (!isInactive)
        {
            Drive();
        }

        // Se o inimigo estiver inativo e o tempo de espera tiver passado, reativa o movimento
        if (isInactive && Time.time - deactivateTime >= deactivateDuration)
        {
            ReactivateEnemy();
        }
    }

    public void Drive()
    {
        if (!destinationReached)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0; // Evita que o carro suba ao mirar no destino
            float destinationDistance = destinationDirection.magnitude;

            // Faz o carro girar suavemente em direção ao destino
            if (destinationDistance >= breakSpeed)
            {
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.fixedDeltaTime));

                // Aumenta gradualmente a velocidade até o máximo
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);

                // Move o carro para frente usando a força do Rigidbody
                rb.linearVelocity = transform.forward * currentSpeed;
            }
            else
            {
                destinationReached = true;
                rb.linearVelocity = Vector3.zero;
            }
        }
    }

    public void LocateDestination(Vector3 newDestination)
    {
        destination = newDestination;
        destinationReached = false;
    }

    public void IncreaseLap()
    {
        currentLap++;
        Debug.Log(" Car " + gameObject.name + " Lap: " + currentLap);
    }

    public void ReduceSpeed(float duration, float multiplier)
    {
        StartCoroutine(ReduceSpeedCoroutine(duration, multiplier));
    }

    private IEnumerator ReduceSpeedCoroutine(float duration, float multiplier)
    {
        float originalMaxSpeed = maxSpeed;
        maxSpeed *= multiplier;

        yield return new WaitForSeconds(duration);

        maxSpeed = originalMaxSpeed;
    }

    // Desativa temporariamente o inimigo, congelando o Rigidbody
    public void DeactivateTemporarily(float duration)
    {
        isInactive = true;               // Marca que o inimigo está inativo
        deactivateDuration = duration;   // Define a duração que o inimigo ficará inativo
        deactivateTime = Time.time;      // Marca o tempo em que a desativação ocorreu

        // Congela a velocidade e a rotação do inimigo
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Debug.Log($"Inimigo {gameObject.name} removido temporariamente! Tempo iniciado: {deactivateTime}");
    }

    // Reativa o inimigo e o deixa voltar ao movimento
    private void ReactivateEnemy()
    {
        // Descongela o movimento do inimigo
        rb.linearVelocity = Vector3.zero;  // Garante que o carro pare antes de retomar o movimento
        isInactive = false;          // Marca que o inimigo não está mais inativo

        Debug.Log($"Inimigo {gameObject.name} reapareceu! Tempo: {Time.time}");
    }
}
