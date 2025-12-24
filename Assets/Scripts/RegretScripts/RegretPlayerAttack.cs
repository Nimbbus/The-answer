using UnityEngine;

public class RegretPlayerAttack : MonoBehaviour
{
    [Header("References")]
    public Collider weaponCollider; // sword collider (assign in Inspector)

    private RegretPlayerHit hitScript; // handles applying damage on hit

    void Start()
    {
        // cache hit handler from collider
        if (weaponCollider != null)
        {
            hitScript = weaponCollider.GetComponent<RegretPlayerHit>();
        }
    }

    // enable collider for light attack and set damage to 30
    public void EnableLightAttackCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            if (hitScript != null)
            {
                hitScript.ResetHit();    // allow one hit this swing
                hitScript.SetDamage(30); // light attack damage
            }
        }
    }

    // enable collider for heavy attack and set damage to 50
    public void EnableHeavyAttackCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            if (hitScript != null)
            {
                hitScript.ResetHit();    // allow one hit this swing
                hitScript.SetDamage(50); // heavy attack damage
            }
        }
    }

    // disable weapon collider after attack frames
    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }
}