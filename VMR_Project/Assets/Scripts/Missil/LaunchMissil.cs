using UnityEngine;

public class LaunchMissil : MonoBehaviour
{
    [SerializeField] private GameObject missilPrefab;
    [SerializeField] private float missilSpeed;

    private float time = 3.0f;
    private float timer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 3.0f;
    }
    
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= time)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                var missil = Instantiate(missilPrefab, transform.position, transform.rotation);
                missil.GetComponent<Rigidbody>().linearVelocity = transform.forward * missilSpeed;
                timer = 0;
            }
        }
    }
}
