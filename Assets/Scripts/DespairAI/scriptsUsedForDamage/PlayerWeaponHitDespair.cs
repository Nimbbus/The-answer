using UnityEngine;

public class PlayerWeaponHitDespair : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damage = 20;
    [SerializeField] private float hitCooldown = 0.3f;

    private float lastHitTime = -999f;
    private Collider weaponCollider;

    void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        if (weaponCollider != null)
        {
            weaponCollider.isTrigger = true;
            weaponCollider.enabled = false;
            Debug.Log($"{gameObject.name}: Player weapon collider initialized and disabled.");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: No collider found on player weapon.");
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
            Debug.Log($"{gameObject.name}: Player weapon collider enabled.");
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
            Debug.Log($"{gameObject.name}: Player weapon collider disabled.");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!weaponCollider.enabled) return;
        if (!other.CompareTag("Boss")) return;

        if (Time.time - lastHitTime < hitCooldown)
        {
            Debug.Log($"{gameObject.name}: Skipped hit — cooldown not expired.");
            return;
        }

        // Directly damage the boss's health script
        DespairSceneHealth bossHealth = other.GetComponent<DespairSceneHealth>();
        if (bossHealth != null)
        {
            bossHealth.TakeDamage(damage);
            lastHitTime = Time.time;
            Debug.Log($"{gameObject.name}: Player hit boss for {damage} damage. Remaining health: {bossHealth.currentHealth}");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: Boss has no DespairSceneHealth component!");
        }
    }
}
