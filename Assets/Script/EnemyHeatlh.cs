using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Loot")]
    public GameObject xpGemPrefab;
    public int xpReward = 20;

    [Header("UI Settings")]
    public GameObject healthBarPrefab; // Drag the "HealthBarCanvas" prefab here
    private FloatingHealthBar healthBar; // We hold a reference to the spawned bar

    void Awake()
    {
        // 1. Spawn the Health Bar automatically when enemy is created
        if (healthBarPrefab != null)
        {
            GameObject barObj = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);

            // 2. Get the script from the new bar object
            healthBar = barObj.GetComponent<FloatingHealthBar>();

            // 3. Tell the bar to follow ME (the enemy)
            if (healthBar != null)
            {
                healthBar.target = this.transform;
            }
        }
    }

    void Start()
    {
        currentHealth = maxHealth;

        // Initialize the bar to full
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Update the visual bar
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        // Update the visual bar
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    void Die()
    {
        // 1. Tell Spawner to allow a new enemy
        EnemySpawner.currentEnemyCount--;

        // 2. Drop XP
        if (xpGemPrefab != null)
        {
            GameObject gem = Instantiate(xpGemPrefab, transform.position, Quaternion.identity);
            if (gem.TryGetComponent<ExpGem>(out ExpGem gemScript))
            {
                gemScript.xpAmount = xpReward;
            }
        }

        // 3. Destroy the Health Bar (so it doesn't stay floating in empty space)
        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }

        // 4. Destroy the Enemy
        Destroy(gameObject);
    }
}