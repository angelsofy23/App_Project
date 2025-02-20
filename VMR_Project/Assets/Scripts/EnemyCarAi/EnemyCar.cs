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

    [Header("Obstacle Avoidance")]
    public float detectionDistance = 5f; // Distância para detectar obstáculos à frente
    public float sideDetectionDistance = 3f; // Distância para verificar espaço lateral
    public float avoidanceForce = 2f; // Força de desvio
    public LayerMask obstacleLayer; // Camada dos obstáculos

    [Header("Destination Var")]
    public Vector3 destination;
    public bool destinationReached;

    [Header("Lap")]
    public int maxLaps;
    public int currentLap;

    [Header("Respawn")]
    public float respawnTimer = 0f;
    public const float respawnTimeThreshold = 5f;

    private Rigidbody rb;
    public bool isInactive = false;
    private float deactivateTime;
    private float deactivateDuration = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        maxLaps = FindObjectOfType<FinishSystem>().maxLaps;
    }

    void Update()
    {
        if (!destinationReached)
        {
            respawnTimer += Time.deltaTime;

            if (respawnTimer >= respawnTimeThreshold)
            {
                //respawn the car
                RespawnAtDestination();
            }
        }
        else
        {
            respawnTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        if (!isInactive)
        {
            DetectAndAvoidObstacles();
            Drive();
        }

        if (isInactive && Time.time - deactivateTime >= deactivateDuration)
        {
            ReactivateEnemy();
        }

       
    }

    void Drive()
    {
        if (!destinationReached)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;
            float destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= breakSpeed)
            {
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.fixedDeltaTime));

                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);
                rb.linearVelocity = transform.forward * currentSpeed;
            }
            else
            {
                destinationReached = true;
                rb.linearVelocity = Vector3.zero;
            }
        }
    }

    void DetectAndAvoidObstacles()
    {
        RaycastHit hit;
        Vector3 forward = transform.forward;

        // Raycast para detectar obstáculos à frente
        if (Physics.Raycast(transform.position, forward, out hit, detectionDistance, obstacleLayer))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.Log("Obstáculo detectado! Tentando desviar...");

                bool canGoRight = !Physics.Raycast(transform.position + transform.right * 0.5f, forward, sideDetectionDistance, obstacleLayer);
                bool canGoLeft = !Physics.Raycast(transform.position - transform.right * 0.5f, forward, sideDetectionDistance, obstacleLayer);

                if (canGoRight)
                {
                    rb.AddForce(transform.right * avoidanceForce, ForceMode.Impulse);
                }
                else if (canGoLeft)
                {
                    rb.AddForce(-transform.right * avoidanceForce, ForceMode.Impulse);
                }
                else
                {
                    rb.linearVelocity *= 0.5f;
                }
            }
        }
    }

    private void RespawnAtDestination()
    {
        respawnTimer = 0f;
        currentSpeed = 5f;

        transform.position = destination;
        destinationReached = false;
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

    public void DeactivateTemporarily(float duration)
    {
        isInactive = true;
        deactivateDuration = duration;
        deactivateTime = Time.time;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Debug.Log($"Inimigo {gameObject.name} removido temporariamente!");
    }

    private void ReactivateEnemy()
    {
        rb.linearVelocity = Vector3.zero;
        isInactive = false;

        Debug.Log($"Inimigo {gameObject.name} reapareceu!");
    }

   

    // Trigger Collider para detectar obstáculos próximos
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Inimigo detectou um obstáculo próximo!");
            ReduceSpeed(2f, 0.5f); // Reduz a velocidade por 2 segundos
        }
    }
}
