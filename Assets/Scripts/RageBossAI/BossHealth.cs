using UnityEngine;
using UnityEngine.AI; // Needed if boss uses NavMeshAgent
using System.Collections; // Needed for coroutines

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 200;
    public int CurrentHealth { get; private set; }

    private Animator animator;
    private bool isDead = false;

    [Header("Dialogue Settings")]
    [Tooltip("Lines of dialogue the boss will speak after dying.")]
    public string[] bossDeathDialogue; // assign these lines in the Inspector

    [Header("Rock Settings")]
    [Tooltip("Rocks blocking the portal that should disappear when boss dies.")]
    public GameObject[] blockingRocks; // assign both rocks in Inspector

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
            animator.SetTrigger("Die");
        }

        // Disable AI logic
        RageAI ai = GetComponent<RageAI>();
        if (ai != null) ai.enabled = false;

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        // Disable combat collider
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Trigger dialogue with delay
        StartCoroutine(ShowDialogueAfterDelay(3f));

        // ✅ Delay rock removal by 3 seconds
        StartCoroutine(RemoveRocksAfterDelay(4f));
    }

    private IEnumerator ShowDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        DialogueManager dm = FindObjectOfType<DialogueManager>();
        if (dm != null && bossDeathDialogue != null && bossDeathDialogue.Length > 0)
        {
            dm.StartDialogue(bossDeathDialogue);
        }
    }

    private IEnumerator RemoveRocksAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RemoveBlockingRocks();
    }

    private void RemoveBlockingRocks()
    {
        if (blockingRocks != null && blockingRocks.Length > 0)
        {
            foreach (GameObject rock in blockingRocks)
            {
                if (rock != null)
                {
                    rock.SetActive(false);
                }
            }
            Debug.Log("Blocking rocks removed!");
        }
    }
}
