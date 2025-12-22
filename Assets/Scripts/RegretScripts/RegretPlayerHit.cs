using UnityEngine;

public class RegretPlayerHit : MonoBehaviour
{
    private int damage = 0;
    private bool hasHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("Boss"))
        {
            RegretHealth regretHealth = other.GetComponent<RegretHealth>();
            if (regretHealth != null)
            {
                regretHealth.TakeDamage(damage);
                Debug.Log("Regret player hit boss for " + damage + " damage.");
                hasHit = true;
            }
        }
    }

    public void ResetHit()
    {
        hasHit = false;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
}
