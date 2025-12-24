using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    public int damage = 10;
    // prevents multiple hits per swing
    private bool hasHit = false;

    private void OnTriggerEnter(Collider other)
    {
      

        // only hit player and only once until reset
        if (!hasHit && other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // apply damage to player
                playerHealth.TakeDamage(damage);
             
                hasHit = true;
            }

        }
    }

    // called to allow the weapon to hit again
    public void ResetHit()
    {
        hasHit = false;
    }
}
