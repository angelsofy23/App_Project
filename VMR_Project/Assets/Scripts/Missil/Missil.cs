using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Missil : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(MissilCooldownCoroutine());
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
            // Parar carro
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
