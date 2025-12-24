using UnityEngine;

public class RegretPlayerHit : MonoBehaviour
{
    private int damage = 0;      // damage applied on hit
    private bool hasHit = false; // prevent multiple hits per swing

    // called when weapon collider hits something
    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return; // already hit this swing

        // only affect objects tagged as Boss
        if (other.CompareTag("Boss"))
        {
            RegretHealth regretHealth = other.GetComponent<RegretHealth>();
            if (regretHealth != null)
            {
                regretHealth.TakeDamage(damage); // apply damage
                hasHit = true; // mark as used for this swing
            }
        }
    }

    // allow the weapon to hit again
    public void ResetHit()
    {
        hasHit = false;
    }

    // set damage amount for next hit
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
}
