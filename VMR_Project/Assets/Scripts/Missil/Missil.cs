using System.Collections;
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
}
