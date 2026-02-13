using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [Header("Settings")]
    public int xpAmount = 10;
    public float magnetSpeed = 10f;

    [Tooltip("How close the player needs to be to magnetize the gem")]
    public float pickupRange = 5f;

    private Transform player;
    private bool isFlyingToPlayer = false;

    void Start()
    {
        // Find the player automatically when the Gem spawns
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // 1. Check if Player is close enough (Magnet Logic)
        // We only check this if we aren't already flying
        if (!isFlyingToPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer < pickupRange)
            {
                isFlyingToPlayer = true;
            }
        }

        // 2. Fly Logic
        if (isFlyingToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, magnetSpeed * Time.deltaTime);

            // 3. Collect Logic
            if (Vector3.Distance(transform.position, player.position) < 0.5f)
            {
                CollectXP();
            }
        }
    }

    void CollectXP()
    {
        // Try to get the stats from the player
        if (player.TryGetComponent<PlayerStats>(out PlayerStats stats))
        {
            stats.AddExp(xpAmount);
        }

        Destroy(gameObject);
    }
}