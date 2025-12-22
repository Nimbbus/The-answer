using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RegretPlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 200f;
    public float currentHealth;

    [Header("UI")]
    public Slider playerHealthSlider;

    [Header("Animation")]
    public Animator playerAnimator; // assign in Inspector

    [Header("Control")]
    public MonoBehaviour movementScript; // assign your player movement script here

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (playerHealthSlider != null)
        {
            playerHealthSlider.maxValue = maxHealth;
            playerHealthSlider.value = currentHealth;
        }

        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<Animator>();
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (playerHealthSlider != null)
        {
            playerHealthSlider.value = currentHealth;
        }

        Debug.Log("Regret player took " + amount + " damage. Current health: " + currentHealth);

        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("GotHit");
        }

        if (currentHealth <= 0)
        {
            isDead = true;
            Debug.Log("Regret player died!");

            if (playerAnimator != null)
            {
                // ✅ Stop all animation states that could override death
                playerAnimator.SetBool("isWalking", false);
                playerAnimator.SetBool("LightAttack", false);
                playerAnimator.SetBool("HeavyAttack", false);
                playerAnimator.SetBool("Dodge", false);
                playerAnimator.ResetTrigger("GotHit");

                // ✅ Trigger death animation
                playerAnimator.SetTrigger("Die");
            }

            if (movementScript != null)
            {
                movementScript.enabled = false;
            }

            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        yield return null; // wait one frame so Animator can process the trigger

        // Wait until Animator enters the Dying state
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("Dying"))
        {
            yield return null;
            stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        }

        Debug.Log("Entered Dying state. Waiting for animation to finish...");

        // Wait for the full length of the Dying animation
        float deathAnimLength = stateInfo.length;
        yield return new WaitForSeconds(deathAnimLength);

        SceneManager.LoadScene("MainMenu");
    }
}
