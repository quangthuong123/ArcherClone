using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyEvolution : MonoBehaviour
{
    [Header("Leveling Settings")]
    public int currentLevel = 1;
    public int maxLevel = 20; // Cap at Level 20

    public float currentXp = 0;
    public float xpToNextLevel = 20f;
    public float passiveXpRate = 5f;

    [Header("Stat Buffs")]
    public int damageAmount = 1;
    public int healthBuff = 2;
    public int damageBuff = 1;

    private EnemyHealth _healthScript;

    void Awake()
    {
        _healthScript = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        // Stop gaining XP if we hit the max level
        if (currentLevel >= maxLevel) return;

        currentXp += passiveXpRate * Time.deltaTime;

        if (currentXp >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentLevel++;
        currentXp = 0;
        xpToNextLevel *= 1.2f;

        // 1. Always buff stats (Health & Damage) every level
        if (_healthScript != null)
        {
            _healthScript.maxHealth += healthBuff;
            _healthScript.Heal(healthBuff);
        }
        damageAmount += damageBuff;

        // 2. Only grow size every 5 levels (5, 10, 15, 20)
        if (currentLevel % 5 == 0)
        {
            transform.localScale *= 1.3f; // Big growth spurt!

            // Visual feedback: Flash red to show they evolved
            GetComponent<Renderer>().material.color = Color.red;
            Debug.Log("BOSS EVOLUTION! Enemy hit Level " + currentLevel);
        }
        else
        {
            // Just a small color tint for normal levels
            GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, (float)currentLevel / maxLevel);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. Try to find the PlayerStats script on the object we hit
            if (collision.gameObject.TryGetComponent<PlayerStats>(out PlayerStats playerStats))
            {
                // 2. Deal the damage!
                playerStats.TakeDamage(damageAmount);

                // Optional: Push the player back slightly (Knockback) so they don't get stuck
                // Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
                // if (playerRb != null) playerRb.AddForce(transform.forward * 5f, ForceMode.Impulse);
                Debug.Log("Enemy (Lvl " + currentLevel + ") hit Player for " + damageAmount + " damage!");
            }
        }
    }
}

