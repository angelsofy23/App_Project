using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Vari�veis relativas � suavidade do movimento e rota��o da c�mera
    public float moveSmoothness;
    public float rotationSmoothness;

    // Offset da posi��o e rota��o da c�mera em rela��o ao carro
    public Vector3 moveOffset;
    public Vector3 rotationOffset;

    // Refer�ncia ao alvo (player) que a c�mera deve seguir
    public Transform carTarget;

    void LateUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        // Calcula a posi��o alvo da c�mera com base na posi��o do carro e no offset
        Vector3 targetPos = new Vector3();
        targetPos = carTarget.TransformPoint(moveOffset);

        // Move suavemente a c�mera para a posi��o alvo
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothness * Time.deltaTime);
    }

    void HandleRotation()
    {
        // Calcula a dire��o da c�mera em rela��o ao carro
        var direction = carTarget.position - transform.position;
        var rotation = new Quaternion();

        // Define a rota��o da c�mera para olhar na dire��o do carro
        rotation = Quaternion.LookRotation(direction + rotationOffset, Vector3.up);

        // Suaviza a transi��o de rota��o para evitar movimentos bruscos
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSmoothness * Time.deltaTime);
    }
}
