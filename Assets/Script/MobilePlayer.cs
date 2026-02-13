using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MobilePlayer : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 720f;

    [Header("Combat")]
    public GameObject arrowPrefab;
    public Transform firePoint;

    // NEW: Reference to stats so we know if we have Multishot/Ricochet
    public PlayerStats playerStats;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    // --- UPDATED: The Shooting Function ---
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (arrowPrefab != null && firePoint != null)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        // 1. Determine stats from PlayerStats
        int arrowCount = 1;
        bool ricochet = false;
        int currentDamage = 1; // Default damage if stats are missing

        if (playerStats != null)
        {
            arrowCount += playerStats.multishotCount; // e.g., 0 upgrades = 1 arrow
            ricochet = playerStats.hasRicochet;
            currentDamage = playerStats.damage;       // Get damage from stats
        }

        // 2. Settings for the Spread (Cone shape)
        float spreadAngle = 15f;

        // Calculate the starting angle so the group is centered
        // Example: 3 arrows = -15, 0, +15 degrees
        float startAngle = -(spreadAngle * (arrowCount - 1)) / 2;

        // 3. Loop to create every arrow
        for (int i = 0; i < arrowCount; i++)
        {
            // Calculate rotation for THIS specific arrow
            float currentAngle = startAngle + (i * spreadAngle);
            Quaternion rotation = transform.rotation * Quaternion.Euler(0, currentAngle, 0);

            // Spawn it!
            GameObject newArrow = Instantiate(arrowPrefab, firePoint.position, rotation);

            // 4. Apply Upgrades to the Arrow Script
            ArrowProjectile arrowScript = newArrow.GetComponent<ArrowProjectile>();
            if (arrowScript != null)
            {
                // Pass the Damage Upgrade
                arrowScript.damage = currentDamage;

                // Pass the Ricochet Upgrade
                if (ricochet)
                {
                    arrowScript.canRicochet = true;
                }
            }
        }
    }
    // ----------------------------------

    void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(_moveInput.x, 0, _moveInput.y) * moveSpeed;
        _rb.linearVelocity = targetVelocity;

        if (_lookInput.sqrMagnitude > 0.05f)
        {
            RotateTowards(_lookInput);
        }
        else if (_moveInput.sqrMagnitude > 0.05f)
        {
            RotateTowards(_moveInput);
        }
    }

    void RotateTowards(Vector2 direction)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }
}