using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Variáveis relativas à suavidade do movimento e rotação da câmera
    public float moveSmoothness;
    public float rotationSmoothness;

    // Offset da posição e rotação da câmera em relação ao carro
    public Vector3 moveOffset;
    public Vector3 rotationOffset;

    // Referência ao alvo (player) que a câmera deve seguir
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
        // Calcula a posição alvo da câmera com base na posição do carro e no offset
        Vector3 targetPos = new Vector3();
        targetPos = carTarget.TransformPoint(moveOffset);

        // Move suavemente a câmera para a posição alvo
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothness * Time.deltaTime);
    }

    void HandleRotation()
    {
        // Calcula a direção da câmera em relação ao carro
        var direction = carTarget.position - transform.position;
        var rotation = new Quaternion();

        // Define a rotação da câmera para olhar na direção do carro
        rotation = Quaternion.LookRotation(direction + rotationOffset, Vector3.up);

        // Suaviza a transição de rotação para evitar movimentos bruscos
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSmoothness * Time.deltaTime);
    }
}
