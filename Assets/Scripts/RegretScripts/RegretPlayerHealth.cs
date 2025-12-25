using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RegretPlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 200f;     // maximum health
    public float currentHealth;        // current health

    [Header("UI")]
    public Slider playerHealthSlider;  // health bar slider

    [Header("Animation")]
    public Animator playerAnimator;    // animator reference

    [Header("Control")]
    public MonoBehaviour movementScript; // movement script to disable on death

    private bool isDead = false;       // death flag

    void Start()
    {
        // initialize health and UI
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
        if (isDead) return; // ignore damage when dead

        // apply damage and clamp value
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // update UI if present
        if (playerHealthSlider != null)
            playerHealthSlider.value = currentHealth;

        // play hit animation
        if (playerAnimator != null)
            playerAnimator.SetTrigger("GotHit");

        // trigger death when health is depleted
        if (currentHealth <= 0)
        {
            isDead = true;

            if (playerAnimator != null)
            {
                // stop other animation states that may override death
                playerAnimator.SetBool("isWalking", false);
                playerAnimator.SetBool("LightAttack", false);
                playerAnimator.SetBool("HeavyAttack", false);
                playerAnimator.SetBool("Dodge", false);
                playerAnimator.ResetTrigger("GotHit");

                // play death animation
                playerAnimator.SetTrigger("Die");
            }

            // disable movement controls
            if (movementScript != null)
                movementScript.enabled = false;

            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        // wait one frame for animator to process the trigger
        yield return null;

        // wait until animator enters the "Dying" state
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("Dying"))
        {
            yield return null;
            stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        }

        // wait for the dying animation to finish
        float deathAnimLength = stateInfo.length;
        yield return new WaitForSeconds(deathAnimLength);

        // return to main menu
        SceneManager.LoadScene("Regret");
    }
}
