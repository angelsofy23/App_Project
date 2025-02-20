using System.Collections;
using UnityEngine;

public class Missil : MonoBehaviour
{
    private EnemyCar enemyCar;
    
    void Awake()
    {
        // Inicia a corrotina que controla o tempo de cooldown do míssil
        StartCoroutine(MissilCooldownCoroutine());
        // Encontra o objeto com a tag "Enemy" e obtém o componente EnemyCar associado ao mesmo
        enemyCar = GameObject.FindWithTag("Enemy").GetComponent<EnemyCar>();
    }

    IEnumerator MissilCooldownCoroutine()
    {
        // Espera por 7 segundos antes de destruir o míssil
        yield return new WaitForSeconds(7);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Inimigo Atingido");
            enemyCar.ReduceSpeed(2f, 0.5f); // Reduz a velocidade por 2 segundos
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
