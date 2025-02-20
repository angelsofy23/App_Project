using System.Collections;
using UnityEngine;

//AQUI � ONDE EST� FEITA A L�GICA DA FSM

// Enum que define os estados dos PowerUps - Dispon�vel, ativo e em indispon�vel
public enum PowerUpState { Available, Activated, Cooldown }

// Enum que define os tipos de PowerUp existentes 
public enum PowerUpType { SpeedBoost, SlowEnemies, RemoveEnemy }

public class PowerUp : MonoBehaviour
{
    // Estado atual do PowerUp - dispon�vel
    public PowerUpState state = PowerUpState.Available;
    public float effectDuration = 5f;
    public float cooldownTime = 10f;

    // Tipo de PowerUp (a funcionalidade que ele ir� realizar)
    public PowerUpType type;

    private MeshRenderer meshRenderer;
    private Collider powerUpCollider;

    private void Start()
    {
        // Pegamos nos componentes para poder desativar a apar�ncia do PowerUp
        meshRenderer = GetComponent<MeshRenderer>();
        powerUpCollider = GetComponent<Collider>();
    }

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

        // Desativar a apar�ncia do PowerUp
        meshRenderer.enabled = false;
        powerUpCollider.enabled = false;
    }

    IEnumerator ApplyEffect(GameObject player)
    {
        switch (type) //A��es baseadas no tipo do PowerUp
        {
            case PowerUpType.SpeedBoost:
                // Se o PowerUp for de aumento de velocidade, aplica o boost no jogador
                player.GetComponent<CarController>().BoostSpeed(effectDuration, 1.5f);
                break;

            case PowerUpType.SlowEnemies:
                // Se o PowerUp for para desacelerar inimigos, reduz a velocidade de todos os inimigos
                foreach (var enemy in FindObjectsOfType<EnemyCar>())
                    enemy.ReduceSpeed(effectDuration, 0.5f);
                break;

            case PowerUpType.RemoveEnemy: //N�o remove, mas desabilita um inimigo aleat�rio durante um tempo determinado
                EnemyCar[] enemies = FindObjectsOfType<EnemyCar>();
                if (enemies.Length > 0)
                {
                    EnemyCar selectedEnemy = enemies[Random.Range(0, enemies.Length)];
                    selectedEnemy.DeactivateTemporarily(effectDuration);
                }
                break;
        }

        yield return new WaitForSeconds(effectDuration);
        StartCoroutine(StartCooldown());
    }

    IEnumerator StartCooldown()
    {
        state = PowerUpState.Cooldown;

        // Espera que o cooldown acabe
        yield return new WaitForSeconds(cooldownTime);

        // Reativa o PowerUp
        state = PowerUpState.Available;
        meshRenderer.enabled = true; // Reativa a apar�ncia
        powerUpCollider.enabled = true; // Reativa a colis�o
    }
}
