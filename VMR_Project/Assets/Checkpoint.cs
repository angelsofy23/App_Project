using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject car = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.transform.GetComponent<CarController>())
        {
            car = other.transform.root.gameObject;
        }
    }

    public bool CarDetected()
    {
        if (car != null)
        {
            return true;
        }

        return false;
    }
}
