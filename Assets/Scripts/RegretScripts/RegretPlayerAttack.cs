using UnityEngine;

public class RegretPlayerAttack : MonoBehaviour
{
    [Header("References")]
    public Collider weaponCollider; // assign sword collider in Inspector

    private RegretPlayerHit hitScript;

    void Start()
    {
        if (weaponCollider != null)
        {
            hitScript = weaponCollider.GetComponent<RegretPlayerHit>();
        }
    }

    // Light Attack (30 dmg)
    public void EnableLightAttackCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            if (hitScript != null)
            {
                hitScript.ResetHit();
                hitScript.SetDamage(30);
            }
            Debug.Log("Light attack collider enabled.");
        }
    }

    // Heavy Attack (50 dmg)
    public void EnableHeavyAttackCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            if (hitScript != null)
            {
                hitScript.ResetHit();
                hitScript.SetDamage(50);
            }
            Debug.Log("Heavy attack collider enabled.");
        }
    }

    // Shared disable method
    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
            Debug.Log("Weapon collider disabled.");
        }
    }
}
