using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int CurrentHealth { get; private set; }

    private Animator animator;

    void Start()
    {
        CurrentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        // ✅ Trigger getting hit animation
        if (animator != null)
        {
            animator.SetTrigger("GotHit");
        }



        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // TODO: Add death animation, disable controls, or reload scene
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > maxHealth)
        {
            CurrentHealth = maxHealth;
        }

        Debug.Log("Player healed by " + amount + ". Current health: " + CurrentHealth);
    }
}
