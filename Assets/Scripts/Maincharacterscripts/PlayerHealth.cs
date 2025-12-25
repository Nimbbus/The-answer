using UnityEngine;
using UnityEngine.SceneManagement; // scene loading

// Manages player health, damage and death
public class PlayerHealth : MonoBehaviour
{
    public static bool IsPlayerDead = false; // global dead flag

    [Header("Health Settings")]
    public int maxHealth = 100; // maximum health
    public int CurrentHealth { get; private set; } // current health

    private Animator animator; // animator reference
    private bool isDead = false; // local dead flag

    void Start()
    {
        // initialize health and cache animator
        CurrentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // ignore damage when dead

        CurrentHealth -= damage; // subtract damage

        if (CurrentHealth > 0)
        {
            // play hit animation if alive
            if (animator != null)
            {
                animator.SetTrigger("GotHit");
            }
        }
        else
        {
            // health depleted -> die
            Die();
        }
    }

    private void Die()
    {
        // mark death and play death animation
        IsPlayerDead = true;
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // ✅ Disable movement and input scripts so player cannot move
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this) // keep PlayerHealth active
                script.enabled = false;
        }

        // ✅ Optionally freeze rigidbody movement if physics is used
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    // Called by an animation event at the end of the death animation
    public void ReturnToMainMenu()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
