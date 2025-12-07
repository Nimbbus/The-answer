using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    public int damage = 10;
    private bool hasHit = false; // ✅ prevents multiple hits per swing

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Weapon collider triggered by: " + other.name);

        if (!hasHit && other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Weapon hit player for " + damage + " damage. Remaining health: " + playerHealth.CurrentHealth);
                hasHit = true; // ✅ block further hits until reset
            }
            else
            {
                Debug.LogWarning("PlayerHealth not found on: " + other.name);
            }
        }
    }

    // ✅ Called by RageAI when starting a new attack
    public void ResetHit()
    {
        hasHit = false;
    }
}
