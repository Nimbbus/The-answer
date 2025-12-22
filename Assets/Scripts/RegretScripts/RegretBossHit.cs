using UnityEngine;

public class RegretBossHit : MonoBehaviour
{
    private int damage = 0;
    private bool hasHit = false;

    private void OnTriggerEnter(Collider other)
    {
        // Only register one hit per collider activation
        if (hasHit) return;

        if (other.CompareTag("Player"))
        {
            RegretPlayerHealth playerHealth = other.GetComponent<RegretPlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Boss hit player for " + damage + " damage.");
                hasHit = true; // ✅ prevents multiple hits until reset
            }
        }
    }

    // Called when collider is enabled for a new attack
    public void ResetHit()
    {
        hasHit = false;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
}
