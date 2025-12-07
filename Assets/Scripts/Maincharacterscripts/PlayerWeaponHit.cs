using UnityEngine;

public class PlayerWeaponHit : MonoBehaviour
{
    private int damage = 20; // default light attack
    private bool hasHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasHit && other.CompareTag("Boss"))
        {
            BossHealth bossHealth = other.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
                Debug.Log("Player hit boss for " + damage + " damage. Remaining health: " + bossHealth.CurrentHealth);
                hasHit = true;
            }
        }
    }

    public void ResetHit()
    {
        hasHit = false;
    }

    // ✅ Called by PlayerAttack script before enabling collider
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
}
