using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    public Collider weaponCollider; // sword collider (assign in Inspector)

    private PlayerWeaponHit hitScript; // damage/hit handler on collider

    void Start()
    {
        // cache the hit script from the collider
        if (weaponCollider != null)
        {
            hitScript = weaponCollider.GetComponent<PlayerWeaponHit>();
        }
    }

    // enable collider for light attack and set damage to 20
    public void EnableLightAttackCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            if (hitScript != null)
            {
                hitScript.ResetHit();    // allow a new hit this swing
                hitScript.SetDamage(20); // set light attack damage
            }
        }
    }

    // enable collider for heavy attack and set damage to 40
    public void EnableHeavyAttackCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            if (hitScript != null)
            {
                hitScript.ResetHit();    // allow a new hit this swing
                hitScript.SetDamage(40); // set heavy attack damage
            }
        }
    }

    // disable the weapon collider after attack frames
    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }
}
