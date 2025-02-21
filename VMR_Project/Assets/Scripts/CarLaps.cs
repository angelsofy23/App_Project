using UnityEngine;
using TMPro;
public class CarLaps : MonoBehaviour
{
    GameObject[] checkpoints;
    GameObject finishLine;
    public int checkpointsTriggered = 0;
    public int maxLaps = 2;
    public int currentLap = 0;
    FinishSystem finishSystem;
    public GameObject winPanel;
    public GameObject losePanel;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI lapsText;
    public bool useTimer = false;
    public float timer = 0f;
    public float maxTime = 2 * 60f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finishSystem = GameObject.Find("FinishLine").GetComponent<FinishSystem>();
        // Look for the gameobject with name "Checkpoints"
        GameObject checkpointsObject = GameObject.Find("Checkpoints");
        // Get all the children of the gameobject
        checkpoints = new GameObject[checkpointsObject.transform.childCount];
        for (int i = 0; i < checkpointsObject.transform.childCount; i++)
        {
            checkpoints[i] = checkpointsObject.transform.GetChild(i).gameObject;
        }
    }

    // on update increase timer
    void Update()
    {
        if (lapsText)
            lapsText.text = "Laps: " + currentLap.ToString() + "/" + maxLaps.ToString();
        if (useTimer && currentLap != maxLaps)
        {
            timer += Time.deltaTime;
            float time_show = maxTime - timer;
            timerText.text = "Time: " + time_show.ToString("F2");
        }
        if (timer > maxTime)
        {
            if (finishSystem)
                finishSystem.EndMission(false);
            else{
                Debug.Log("You lost!");
                losePanel.SetActive(true);
            }
            timer = 0f;
        }
    }

    void enableCheckpoints()
    {
        checkpointsTriggered = 0;
        foreach (GameObject checkpoint in checkpoints)
        {
            checkpoint.SetActive(true);
        }
    }

    // If triggers with the checkpoint
    void OnTriggerEnter(Collider other)
    {
        // Only check collisions with objects tagged as "Checkpoint"
        if (other.CompareTag("Checkpoint"))
        {
            // Disable the triggered checkpoint
            other.gameObject.SetActive(false);
            
            // Increment the checkpointsTriggered
            checkpointsTriggered++;

            Debug.Log("Checkpoint " + checkpointsTriggered + " triggered!");
            
        }

        // if tag == "Finish"
        if (other.CompareTag("Finish"))
        {
            // If all checkpoints were taken and current lap is equal to max laps
            if (checkpointsTriggered == checkpoints.Length)
            {
                currentLap++;
                if (currentLap == maxLaps)
                {
                    // If it's the last lap, print "You won!"
                    if (finishSystem)
                        finishSystem.EndMission(true);
                    else{
                        Debug.Log("You won!");
                        winPanel.SetActive(true);
                    }
                }
                // Increment the laps
                Debug.Log("Lap " + currentLap + " completed!");
                // Reset the checkpointsTriggered
                enableCheckpoints();
            }
        }
    }
}
