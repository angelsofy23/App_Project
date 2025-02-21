using TMPro;
using UnityEngine;

public class WinningPost : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI countLaps;

    private CarController car; 
    
    private Checkpoint[] checkpoints;
    private bool gameOver;
    
    private float maxTime = 180f; // 3 min
    private float remainingTime;
    
    public bool gamePaused;
    
    private void Awake()
    {
        // Encontra o objeto com a tag "Player" e obtém o componente CarController associado ao mesmo
        car = GameObject.FindWithTag("Player").GetComponent<CarController>();

        // Encontra todos os objetos do tipo Checkpoint na cena
        checkpoints = FindObjectsOfType<Checkpoint>();
        
        // Inicializa o tempo restante com o valor máximo de tempo permitido para a corrida (maxTime).
        remainingTime = maxTime;
        
        // Atualiza o texto que exibe a quantidade de voltas (laps) do jogador.
        countLaps.text = $"{car.currentLap} / {car.maxLaps}";
    }

    private void Update()
    {
        // Verifica se o jogo não acabou e se o jogo não está pausado antes de executar o restante código
        if (!gameOver || !gamePaused)
        {
            // Reduz o tempo restante, subtraindo o tempo que passou desde o último quadro (frame)
            remainingTime -= Time.deltaTime;

            // Calcula os minutos e segundos restantes
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            // Atualiza o texto do cronômetro, formatando para mostrar o tempo no formato "MM:SS"
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (remainingTime <= 0f)   // Verifica se o tempo restante chegou a zero ou se ficou negativo
            {
                remainingTime = 0f;  // Garante que o tempo restante não ficará negativo
                EndGame("Acabou o Tempo");
            } 
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o carro que entrou no trigger é o jogador e valida as passagens pelos checkpoints
        foreach (Checkpoint ch in checkpoints)
        {
            if (!ch.CarDetected())  // Verifica se o carro já passou por este checkpoint. Se não, é uma volta inválida
            {
                Debug.Log("Volta Inválida");

                // Reseta o checkpoint e impede que a volta seja considerada válida
                ResetCheckpoint();
                
                return;
            }
        }

        // Se o carro passou por todos os checkpoints corretamente, aumenta a contagem da volta
        car.IncreaseLap();

        // Atualiza o texto da UI com a quantidade de voltas completadas e o total de voltas
        countLaps.text = $"{car.currentLap} / {car.maxLaps}";

        if (car.currentLap > car.maxLaps && !gameOver)  // Verifica se o carro completou todas as voltas e se o jogo ainda não acabou
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
