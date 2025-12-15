using UnityEngine;

public class BossWeaponHitDespair : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damage = 40;
    [SerializeField] private float hitCooldown = 0.5f;

    private float lastHitTime = -999f;
    private Collider weaponCollider;

    void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        if (weaponCollider != null)
        {
            weaponCollider.isTrigger = true;
            weaponCollider.enabled = false;
            Debug.Log($"{gameObject.name}: Weapon collider initialized and disabled.");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: No collider found on weapon.");
        }
    }

    /// <summary>
    /// Called by animation event at the start of the swing.
    /// </summary>
    public void EnableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            lastHitTime = -999f; // reset cooldown
            Debug.Log($"{gameObject.name}: Weapon collider enabled.");
        }
    }

    /// <summary>
    /// Called by animation event at the end of the swing.
    /// </summary>
    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
            Debug.Log($"{gameObject.name}: Weapon collider disabled.");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!weaponCollider.enabled) return;
        if (!other.CompareTag("Player")) return;

        if (Time.time - lastHitTime < hitCooldown)
        {
            Debug.Log($"{gameObject.name}: Skipped hit — cooldown not expired.");
            return;
        }

        // Directly damage the player's health script
        DespairSceneHealth playerHealth = other.GetComponent<DespairSceneHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            lastHitTime = Time.time;
            Debug.Log($"{gameObject.name}: Boss hit player for {damage} damage. Remaining health: {playerHealth.currentHealth}");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: Player has no DespairSceneHealth component!");
        }
    }
}
