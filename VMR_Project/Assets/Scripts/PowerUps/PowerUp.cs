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

    private MeshRenderer meshRenderer;
    private Collider powerUpCollider;

    private void Start()
    {
        // Pegamos os componentes para poder desativar a aparência do PowerUp
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

        // Desativar a aparência do PowerUp
        meshRenderer.enabled = false;
        powerUpCollider.enabled = false;
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

        yield return new WaitForSeconds(effectDuration);
        StartCoroutine(StartCooldown());
    }

    IEnumerator StartCooldown()
    {
        state = PowerUpState.Cooldown;

        // Espera o cooldown acabar
        yield return new WaitForSeconds(cooldownTime);

        // Reativa o PowerUp
        state = PowerUpState.Available;
        meshRenderer.enabled = true; // Reativa a aparência
        powerUpCollider.enabled = true; // Reativa a colisão
    }
}
