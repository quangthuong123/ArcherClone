using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArrowProjectile : MonoBehaviour
{
    [Header("Arrow Settings")]
    public float speed = 20f;
    public float lifetime = 3f;
    public int damage = 1;

    // --- NEW: This fixes the error ---
    [HideInInspector]
    public bool canRicochet = false; // The Player script sets this to true
    private int _bounceCount = 0;
    // ---------------------------------

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // 1. Destroy after X seconds (failsafe)
        Destroy(gameObject, lifetime);

        // 2. Fly Forward
        // Note: linearVelocity is for Unity 6. If you get an error, change to 'velocity'
        _rb.linearVelocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 1. Ignore Player
        if (collision.gameObject.CompareTag("Player")) return;

        // 2. Hit an Enemy?
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // Arrows always break on enemies
            return;
        }

        // 3. Hit a Wall? (Ricochet Logic)
        if (collision.gameObject.CompareTag("Wall"))
        {
            // If we have the upgrade AND haven't bounced too much...
            if (canRicochet && _bounceCount < 2)
            {
                _bounceCount++;
                return; // DO NOT DESTROY! Let it bounce.
            }
        }

        // 4. Hit anything else (or ran out of bounces) -> Destroy
        Destroy(gameObject);
    }
}