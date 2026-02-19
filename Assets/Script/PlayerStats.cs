using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    // ==========================================
    //              1. LEVEL / XP SYSTEM
    // ==========================================
    [Header("XP System")]
    public int currentLevel = 1;
    public float currentExp = 0;
    public float expToNextLevel = 100;
    public float passiveXpRate = 5f;
    [Header("Combat Upgrades")]
    public int multishotCount = 0; // 0 = normal, 1 = double shot, etc.
    public bool hasRicochet = false;
    public bool hasFire = false;
    public bool hasIce = false;
    [Header("Combat Stats")]
    public int damage = 1; // <--- ADD THIS VARIABLE!
    [Tooltip("The Yellow Slider stuck to the top of the screen")]
    public Slider xpSlider;

    // ==========================================
    //              2. HEALTH SYSTEM
    // ==========================================
    [Header("Health System")]
    public int maxHealth = 100;
    public int currentHealth;

    [Tooltip("The Red Slider floating above the player's head")]
    public FloatingHealthBar healthBar;

    void Start()
    {
        // Initialize Health
        currentHealth = maxHealth;

        // Initialize the Health Bar visual
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    void Update()
    {
        // Passive XP Gain over time
        AddExp(passiveXpRate * Time.deltaTime);
    }

    // --- XP FUNCTIONS ---
    public void AddExp(float amount)
    {
        currentExp += amount;

        if (xpSlider != null)
        {
            xpSlider.value = currentExp / expToNextLevel;
        }

        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    // Add variable at top
    public LevelUpManager levelManager;

    void LevelUp()
    {
        currentLevel++;
        currentExp = 0;
        expToNextLevel *= 1.2f;
        if (xpSlider != null) xpSlider.value = 0;

        // CALL THE MANAGER
        if (levelManager != null)
        {
            levelManager.ShowLevelUpOptions();
        }
    }

    // --- NEW HEALTH FUNCTIONS ---
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player HP: " + currentHealth);

        // Update the Floating Bar
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    // Add this to PlayerStats.cs
    public void Heal(int amount)
    {
        currentHealth += amount;

        // Don't overheat (go above max)
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Update UI
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    void Die()
    {
        Debug.Log("GAME OVER");
        Time.timeScale = 0; // Pause everything
        // TODO: Show Game Over Screen
    }
}