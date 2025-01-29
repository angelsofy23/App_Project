using System.Collections;
using UnityEngine;

public enum PowerUpState { Available, Activated, Cooldown }
public enum PowerUpType { SpeedBoost, SlowEnemies, RemoveEnemy }

public class PowerUp : MonoBehaviour
{
    public PowerUpState state = PowerUpState.Available;
    public float effectDuration = 5f;
    public float cooldownTime = 10f;
    public PowerUpType type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && state == PowerUpState.Available)
        {
            ActivatePowerUp(other.gameObject);
        }
    }

    void ActivatePowerUp(GameObject player)
    {
        state = PowerUpState.Activated;
        StartCoroutine(ApplyEffect(player));
    }

    IEnumerator ApplyEffect(GameObject player)
    {
        switch (type)
        {
            case PowerUpType.SpeedBoost:
                player.GetComponent<CarController>().BoostSpeed(effectDuration, 1.5f);
                break;

            case PowerUpType.SlowEnemies:
                foreach (var enemy in FindObjectsOfType<EnemyCar>())
                    enemy.ReduceSpeed(effectDuration, 0.5f);
                break;

            case PowerUpType.RemoveEnemy:
                EnemyCar[] enemies = FindObjectsOfType<EnemyCar>();
                if (enemies.Length > 0)
                {
                    EnemyCar selectedEnemy = enemies[Random.Range(0, enemies.Length)];
                    selectedEnemy.DeactivateTemporarily(effectDuration);
                }
                break;
        }

        // Não usamos mais WaitForSeconds aqui
        // Vamos definir manualmente a reativação do inimigo usando Time.time
        float startTime = Time.time;  // Marca o tempo inicial da aplicação do efeito
        while (Time.time - startTime < effectDuration)
        {
            yield return null;  // Espera sem bloquear a execução
        }

        // Agora começa o cooldown do PowerUp
        StartCoroutine(StartCooldown());
    }

    IEnumerator StartCooldown()
    {
        state = PowerUpState.Cooldown;

        // Log de depuração para o cooldown
        Debug.Log("Cooldown iniciado!");

        // Aguarda o tempo de cooldown para que o PowerUp se torne disponível novamente
        float cooldownStartTime = Time.time; // Marca o tempo inicial do cooldown
        while (Time.time - cooldownStartTime < cooldownTime)
        {
            yield return null;  // Espera sem bloquear a execução
        }

        state = PowerUpState.Available;

        // Log de depuração para o fim do cooldown
        Debug.Log("Cooldown terminado, PowerUp disponível novamente.");
    }
}
