using UnityEngine;

public class BossWeaponHitDespair : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damage = 40;         // damage applied to player
    [SerializeField] private float hitCooldown = 0.5f; // min time between hits

    private float lastHitTime = -999f; // time of last applied hit
    private Collider weaponCollider;   // collider used for hit detection

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

    // enable collider during attack frames and reset hit timer
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
        // only proceed when collider active and hitting the player
        if (!weaponCollider.enabled) return;
        if (!other.CompareTag("Player")) return;

        // enforce cooldown between hits
        if (Time.time - lastHitTime < hitCooldown)
        {
            return;
        }

        // apply damage if target has health component
        DespairSceneHealth playerHealth = other.GetComponent<DespairSceneHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            lastHitTime = Time.time;
        }
    }
}
