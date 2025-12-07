using UnityEngine;
using UnityEngine.AI; // Needed if boss uses NavMeshAgent

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 200;
    public int CurrentHealth { get; private set; }

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        CurrentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Prevent damage after death

        CurrentHealth -= damage;
        Debug.Log("Boss took " + damage + " damage. Current health: " + CurrentHealth);

        if (CurrentHealth > 0)
        {
            // ✅ Flinch animation while alive
            if (animator != null)
            {
                animator.SetTrigger("GotHit");
            }
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Boss died!");

        if (animator != null)
        {
            animator.SetTrigger("Die"); // ✅ plays death animation
        }

        // Disable AI logic so boss stops moving/attacking
        RageAI ai = GetComponent<RageAI>();
        if (ai != null) ai.enabled = false;

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        // ✅ Disable combat collider safely
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

    
    }
}
