using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RegretHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 700f;
    public float currentHealth;

    [Header("UI")]
    public Slider bossHealthSlider;

    [Header("AI Control")]
    public RegretAI bossAI; // assign your AI script in Inspector

    [Header("Final Words Dialogue")]
    [TextArea]
    public string bossFinalWords; // write the boss's last words in Inspector
    public GameObject dialoguePanel; // assign your existing DialoguePanel GameObject
    public TextMeshProUGUI dialogueText; // assign the TMP text component inside the panel

    [Header("Scene Objects")]
    public GameObject blockingRock; // assign rock prefab in Inspector

    private bool isDead = false;
    private bool waitingForInput = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (bossHealthSlider != null)
        {
            bossHealthSlider.maxValue = maxHealth;
            bossHealthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogWarning("BossHealthSlider not assigned in RegretHealth.");
        }
    }

    void Update()
    {
        if (waitingForInput && Input.GetKeyDown(KeyCode.E))
        {
            ResumeGameAfterDialogue();
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (bossHealthSlider != null)
        {
            bossHealthSlider.value = currentHealth;
        }

        Debug.Log("Regret boss took " + amount + " damage. Current health: " + currentHealth);

        if (bossAI != null)
        {
            bossAI.OnHit();
        }

        if (currentHealth <= 0)
        {
            isDead = true;
            Debug.Log("Regret boss died!");

            if (bossAI != null)
            {
                bossAI.OnDeath();
            }

            StartCoroutine(ShowFinalWords());
        }
    }

    IEnumerator ShowFinalWords()
    {
        yield return new WaitForSeconds(2f);

        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = bossFinalWords;

            // ✅ Freeze game
            Time.timeScale = 0f;
            waitingForInput = true;

            Debug.Log("Final words displayed. Waiting for player to press E...");
        }
    }

    void ResumeGameAfterDialogue()
    {
        // ✅ Unfreeze game
        Time.timeScale = 1f;
        waitingForInput = false;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        if (blockingRock != null)
        {
            Destroy(blockingRock);
            Debug.Log("Blocking rock removed after boss dialogue.");
        }
    }
}
