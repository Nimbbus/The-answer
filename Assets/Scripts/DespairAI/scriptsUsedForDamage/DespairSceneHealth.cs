using UnityEngine;

public class DespairSceneHealth : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    [Header("References")]
    public Animator animator;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead)
        {
            Debug.Log($"{gameObject.name}: Ignored damage because already dead.");
            return;
        }

        currentHealth -= amount;
        Debug.Log($"{gameObject.name}: Took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }
        else
        {
            if (animator != null)
            {
                Debug.Log($"{gameObject.name}: Attempting to trigger 'GettingHit'.");
                animator.SetTrigger("GettingHit");
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}: Animator is missing.");
            }
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name}: Died.");

        if (animator != null)
            animator.SetTrigger("Die");

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.isStopped = true;

        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }
}
