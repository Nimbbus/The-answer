using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class DespairSceneHealth : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    [Header("References")]
    public Animator animator;

    [Header("Optional Objects")]
    public GameObject rockToHide;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue Settings")]
    [TextArea(3, 5)]
    public string bossDeathDialogue = "The darkness fades... but despair lingers.";

    private bool isDead = false;
    private bool dialogueActive = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;

            // ✅ If this is the PLAYER, wait for death animation then reload
            if (CompareTag("Player"))
            {
                StartCoroutine(RespawnAfterDeathAnimation());
                return;
            }

            // ✅ If this is the BOSS, show death dialogue and freeze
            Die();
        }
        else
        {
            if (animator != null)
                animator.SetTrigger("GettingHit");

            DespairAI ai = GetComponent<DespairAI>();
            if (ai != null) ai.OnHit();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
            animator.SetTrigger("Die");

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.isStopped = true;

        var col = GetComponent<Collider>();
        if (col != null && !(col is CharacterController))
            col.enabled = false;

        MainCharacterController movement = GetComponent<MainCharacterController>();
        if (movement != null) movement.isDead = true;

        DespairAI ai = GetComponent<DespairAI>();
        if (ai != null) ai.OnDeath();

        if (rockToHide != null)
            rockToHide.SetActive(false);

        StartCoroutine(ShowDialogueAfterDelay(1f));
    }

    IEnumerator ShowDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
            if (dialogueText != null)
                dialogueText.text = bossDeathDialogue;

            dialogueActive = true;
            Time.timeScale = 0f;
        }
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            dialoguePanel.SetActive(false);
            dialogueActive = false;
            Time.timeScale = 1f;
        }
    }

    // ✅ Coroutine waits for death animation before respawn
    IEnumerator RespawnAfterDeathAnimation()
    {
        if (animator != null)
        {
            // Wait until the "Die" animation finishes
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length);
        }

        Respawn();
    }

    void Respawn()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
