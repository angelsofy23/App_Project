using TMPro;
using UnityEngine;

public class WinningPost : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI countLaps;

    public bool gamePaused;

    private CarController car; 
    
    private Checkpoint[] checkpoints;
    private bool gameOver;
    
    private float maxTime = 180f; // 3 min
    private float remainingTime;
    
    private void Awake()
    {
        car = GameObject.FindWithTag("Player").GetComponent<CarController>();
        
        checkpoints = FindObjectsOfType<Checkpoint>();
        remainingTime = maxTime;
        countLaps.text = $"{car.currentLap} / {car.maxLaps}";
    }

    private void Update()
    {
        if (!gameOver || !gamePaused)
        {
            remainingTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                EndGame("Acabou o Tempo");
            } 
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (Checkpoint ch in checkpoints)
        {
            if (!ch.CarDetected())
            {
                Debug.Log("Volta Inválida");

                ResetCheckpoint();
                
                return;
            }
        }
        
        car.IncreaseLap();

        countLaps.text = $"{car.currentLap} / {car.maxLaps}";

        if (car.currentLap > car.maxLaps && !gameOver)
        {
            gameOver = true;

            EndGame("Acabou a Corrida");
        }
        
        ResetCheckpoint();
    }

    void EndGame(string message)
    {
        Debug.Log(message);
    }

    void ResetCheckpoint()
    {
        foreach (Checkpoint chp in checkpoints)
        {
            chp.car = null;
        }
    }
}
