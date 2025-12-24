using UnityEngine;

public class PlayerWeaponHit : MonoBehaviour
{
    private int damage = 20; // damage applied on hit
    private bool hasHit = false; // prevent multiple hits per swing

    // called when weapon collider touches another collider
    private void OnTriggerEnter(Collider other)
    {
        // only hit once per swing and only hit objects tagged "Boss"
        if (!hasHit && other.CompareTag("Boss"))
        {
            BossHealth bossHealth = other.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                // apply damage
                bossHealth.TakeDamage(damage);
                hasHit = true;
            }
        }
    }

    // allow this weapon to hit again (called before next swing)
    public void ResetHit()
    {
        hasHit = false;
    }

    // set damage amount for next hit (called by attacker)
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
}
