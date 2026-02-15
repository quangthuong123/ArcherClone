using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 1; // This will be set by the player when shooting
    public bool canRicochet = false;
    private int bounceCount = 0;
    private int maxBounces = 2;

    void Start()
    {
        // Destroy arrow after 5 seconds so they don't lag the game
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 1. Check if we hit an Enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Uses the upgraded damage!
            }
            Destroy(gameObject);
        }
        // 2. Check if we hit a Wall
        else if (collision.gameObject.CompareTag("Wall"))
        {
            if (canRicochet && bounceCount < maxBounces)
            {
                bounceCount++;
                // We don't destroy, the Physic Material handles the bounce direction
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Hit anything else (Floor, etc.)
            Destroy(gameObject);
        }
    }
}