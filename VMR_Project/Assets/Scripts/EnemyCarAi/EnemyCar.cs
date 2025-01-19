using UnityEngine;

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

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        // Adicionando restri��es para evitar que o carro rode para cima
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        Drive();
    }

    public void Drive()
    {
        if (!destinationReached)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0; // Evita que o carro suba ao mirar no destino
            float destinationDistance = destinationDirection.magnitude;

            // Faz o carro girar suavemente em dire��o ao destino
            if (destinationDistance >= breakSpeed)
            {
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.fixedDeltaTime));

                // Aumenta gradualmente a velocidade at� o m�ximo
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);

                // Move o carro para frente usando a for�a do Rigidbody
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
}
