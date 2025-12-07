using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene loading

public class PlayerHealth : MonoBehaviour
{
    public static bool IsPlayerDead = false; // ✅ global flag
    [Header("Health Settings")]
    public int maxHealth = 100;
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
        Debug.Log("Player took " + damage + " damage. Current health: " + CurrentHealth);

        if (CurrentHealth > 0)
        {
            // ✅ Only play GotHit if still alive
            if (animator != null)
            {
                animator.SetTrigger("GotHit");
                Debug.Log("GotHit trigger set on Animator.");
            }
        }
        else
        {
            // ✅ Health is 0 or less → Die
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player died!");

        if (animator != null)
        {
            animator.SetTrigger("Die"); // ✅ plays death animation
        }

     
    }

    // ✅ Called by an Animation Event at the END of the death animation
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace with your actual menu scene name
    }
}
