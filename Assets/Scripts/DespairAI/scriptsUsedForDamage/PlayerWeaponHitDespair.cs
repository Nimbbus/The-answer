using UnityEngine;

public class PlayerWeaponHitDespair : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damage = 20;         // damage dealt to boss
    [SerializeField] private float hitCooldown = 0.3f; // minimum time between hits

    private float lastHitTime = -999f; // last time damage was applied
    private Collider weaponCollider;   // weapon collider used for hit detection

    void Awake()
    {
        // cache collider, make it a trigger and start disabled
        weaponCollider = GetComponent<Collider>();
        if (weaponCollider != null)
        {
            weaponCollider.isTrigger = true;
            weaponCollider.enabled = false;
        }
    }

    // enable collider during attack frames and reset timer
    public void EnableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            lastHitTime = -999f;
        }
    }

    // disable collider outside attack frames
    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        // only proceed when collider active and target is boss
        if (!weaponCollider.enabled) return;
        if (!other.CompareTag("Boss")) return;

        // enforce cooldown between hits
        if (Time.time - lastHitTime < hitCooldown)
        {
            return;
        }

        // apply damage if boss has health component
        DespairSceneHealth bossHealth = other.GetComponent<DespairSceneHealth>();
        if (bossHealth != null)
        {
            bossHealth.TakeDamage(damage);
            lastHitTime = Time.time;
        }
    }
}
