using System.Collections;
using UnityEngine;

public class Missil : MonoBehaviour
{
    private EnemyCar enemyCar;
    
    void Awake()
    {
        StartCoroutine(MissilCooldownCoroutine());
        enemyCar = GameObject.FindWithTag("Enemy").GetComponent<EnemyCar>();
    }

    IEnumerator MissilCooldownCoroutine()
    {
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
