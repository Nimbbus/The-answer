using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RegretHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 700f;           // max HP
    public float currentHealth;              // current HP

    [Header("UI")]
    public Slider bossHealthSlider;          // health bar slider

    [Header("AI Control")]
    public RegretAI bossAI;                  // boss AI reference

    [Header("Final Words Dialogue")]
    [TextArea]
    public string bossFinalWords;            // final dialogue text
    public GameObject dialoguePanel;         // dialogue UI panel
    public TextMeshProUGUI dialogueText;     // dialogue text element

    [Header("Scene Objects")]
    public GameObject blockingRock;          // rock to remove after dialogue

    private bool isDead = false;             // death flag
    private bool waitingForInput = false;    // waiting for player input

    void Start()
    {
        // initialize health and UI
        currentHealth = maxHealth;

        if (bossHealthSlider != null)
        {
            bossHealthSlider.maxValue = maxHealth;
            bossHealthSlider.value = currentHealth;
        }
    }

    void Update()
    {
        // resume when player confirms final dialogue
        if (waitingForInput && Input.GetKeyDown(KeyCode.E))
        {
            ResumeGameAfterDialogue();
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return; // ignore after death

        // apply and clamp damage
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // update UI if present
        if (bossHealthSlider != null)
        {
            bossHealthSlider.value = currentHealth;
        }

        // notify AI about being hit
        if (bossAI != null)
        {
            bossAI.OnHit();
        }

        // handle death
        if (currentHealth <= 0)
        {
            isDead = true;

            if (bossAI != null)
            {
                bossAI.OnDeath();
            }

            StartCoroutine(ShowFinalWords());
        }
    }

    IEnumerator ShowFinalWords()
    {
        // short delay before showing final text
        yield return new WaitForSeconds(2f);

        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = bossFinalWords;

            // pause gameplay and wait for player input
            Time.timeScale = 0f;
            waitingForInput = true;
        }
    }

    void ResumeGameAfterDialogue()
    {
        // unpause and clean up
        Time.timeScale = 1f;
        waitingForInput = false;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        // remove blocking rock if assigned
        if (blockingRock != null)
        {
            Destroy(blockingRock);
        }
    }
}
