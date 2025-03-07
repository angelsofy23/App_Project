using UnityEngine;
using System.Collections;

public class EnemyCar : MonoBehaviour
{
    [Header("Car Engine")]
    public float maxSpeed = 10f; // Velocidade máxima dos inimigos
    public float currentSpeed;  // Velocidade atual
    public float acceleration = 1f; // Taxa de aceleração
    public float turningSpeed = 30f; // Velocidade de giro
    public float breakSpeed = 2f; // Velocidade de travagem

    [Header("Obstacle Avoidance")]
    public float detectionDistance = 5f; // Distância para detectar obstáculos à frente
    public float sideDetectionDistance = 3f; // Distância para verificar o espaço lateral
    public float avoidanceForce = 2f; // Força de desvio

    public LayerMask obstacleLayer; // Camada dos obstáculos

    [Header("Destination Var")]
    public Vector3 destination; // Posição de destino dos carros inimigos
    public bool destinationReached; // Indica se o destino foi alcançado

    [Header("Lap")]
    public int maxLaps; // Número máximo de voltas
    public int currentLap; //Volta atual

    [Header("Respawn")]
    public float respawnTimer = 0f; // Temporizador de respawn
    public const float respawnTimeThreshold = 10f; // Tempo para o respawn

    private Rigidbody rb;
    public bool isInactive = false;  // Indica se o carro está inativo
    private float deactivateTime;
    private float deactivateDuration = 5f; // Duração da desativação

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // Ativa a gravidade

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;  // Restringe a rotação

        maxLaps = FindObjectOfType<FinishSystem>().maxLaps; // Obtém o número máximo de voltas
    }

    void Update()
    {
        if (!destinationReached)
        {
            respawnTimer += Time.deltaTime;

            if (respawnTimer >= respawnTimeThreshold)
            {
                // Respawna o carro na posição do destino
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
            Vector3 destinationDirection = destination - transform.position; // Direção até o destino
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
                    rb.linearVelocity *= 0.5f; // Reduz a velocidade se não puder desviar
                }
            }
        }
    }

    private void RespawnAtDestination()
    {
        // Reinicia o temporizador de respawn.
        respawnTimer = 0f;
        currentSpeed = 5f;

        // Move o carro para a posição do destino.
        transform.position = destination;
        // Marca que o destino ainda não foi alcançado, permitindo que o carro continue o seu movimento
        destinationReached = false;
    }

    public void LocateDestination(Vector3 newDestination)
    {
        // Atualiza a posição de destino com o novo local fornecido
        destination = newDestination;
        destinationReached = false;
    }

    public void IncreaseLap()
    {
        currentLap++;

        // Exibe na consola a nova volta do carro, identificando-o pelo nome do GameObject
        Debug.Log(" Car " + gameObject.name + " Lap: " + currentLap);
    }

    public void ReduceSpeed(float duration, float multiplier)
    {
        // Inicia uma corrotina para aplicar a redução de velocidade, durante um tempo determinado
        StartCoroutine(ReduceSpeedCoroutine(duration, multiplier));
    }

    private IEnumerator ReduceSpeedCoroutine(float duration, float multiplier)
    {
        float originalMaxSpeed = maxSpeed;
        // Aplica o fator de redução multiplicando a velocidade máxima pelo valor fornecido.
        maxSpeed *= multiplier;

        yield return new WaitForSeconds(duration);

        // Retorna a velocidade máxima ao valor original
        maxSpeed = originalMaxSpeed;
    }

    public void DeactivateTemporarily(float duration)
    {
        isInactive = true;
        deactivateDuration = duration;
        deactivateTime = Time.time;

        // Zera a velocidade linear e angular para parar completamente o carro
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
