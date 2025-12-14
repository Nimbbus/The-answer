using UnityEngine;

public class DespairSceneHealth : MonoBehaviour
{
    [Header("Settings")]
    public string targetTag;       // "Player" for boss weapon, "Enemy" for player weapon
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public float damageAmount = 20f;

    [Header("References")]
    public Animator animator;      // optional, for death/hit animations

    private bool isDead;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            if (animator != null)
                animator.SetTrigger("GettingHit");
        }
    }

    void Die()
    {
        isDead = true;
        if (animator != null)
            animator.SetTrigger("Die");

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.isStopped = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag(targetTag))
        {
            DespairSceneHealth otherHealth = other.GetComponent<DespairSceneHealth>();
            if (otherHealth != null)
            {
                otherHealth.TakeDamage(damageAmount);
            }
        }
    }
}
