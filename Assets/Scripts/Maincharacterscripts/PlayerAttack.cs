using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    public Collider weaponCollider; // assign sword collider in Inspector

    private PlayerWeaponHit hitScript;

    void Start()
    {
        if (weaponCollider != null)
        {
            hitScript = weaponCollider.GetComponent<PlayerWeaponHit>();
        }
    }

    // ✅ Light Attack (20 dmg)
    public void EnableLightAttackCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            if (hitScript != null)
            {
                hitScript.ResetHit();
                hitScript.SetDamage(20);
            }
            Debug.Log("Light attack collider enabled.");
        }
    }

    // ✅ Heavy Attack (40 dmg)
    public void EnableHeavyAttackCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            if (hitScript != null)
            {
                hitScript.ResetHit();
                hitScript.SetDamage(40);
            }
            Debug.Log("Heavy attack collider enabled.");
        }
    }

    // ✅ Shared disable method
    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
            Debug.Log("Weapon collider disabled.");
        }
    }
}
