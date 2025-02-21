using System.Collections;
using UnityEngine;

public class Missil : MonoBehaviour
{    
    void Awake()
    {
        // Inicia a corrotina que controla o tempo de cooldown do m�ssil
        StartCoroutine(MissilCooldownCoroutine());
    }

    IEnumerator MissilCooldownCoroutine()
    {
        // Espera por 7 segundos antes de destruir o m�ssil
        yield return new WaitForSeconds(7);
        Destroy(gameObject);
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")){
            Debug.Log("Inimigo Atingido");
            var enemyCar = other.gameObject.GetComponent<EnemyCar>();
            enemyCar.ReduceSpeed(5f, 0f); // Reduz a velocidade por 2 segundos
        }

        if (!other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
