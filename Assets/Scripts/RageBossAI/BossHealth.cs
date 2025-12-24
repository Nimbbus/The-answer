using UnityEngine;
using UnityEngine.AI; // NavMesh support
using System.Collections; // coroutines

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 200; // max HP
    public int CurrentHealth { get; private set; } // current HP

    private Animator animator; // animator reference
    private bool isDead = false; // death flag

    [Header("Dialogue Settings")]
    [Tooltip("Lines of dialogue the boss will speak after dying.")]
    public string[] bossDeathDialogue; // death lines

    [Header("Rock Settings")]
    [Tooltip("Rocks blocking the portal that should disappear when boss dies.")]
    public GameObject[] blockingRocks; // rocks to remove on death

    void Start()
    {
        // initialize health and cache animator
        CurrentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // ignore after death

        // apply damage
        CurrentHealth -= damage;

        if (CurrentHealth > 0)
        {
            // play hit animation
            if (animator != null)
            {
                animator.SetTrigger("GotHit");
            }
        }
        else
        {
            // handle death
            Die();
        }
    }

    private void Die()
    {
        // mark dead and play death animation
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // disable AI behavior
        RageAI ai = GetComponent<RageAI>();
        if (ai != null) ai.enabled = false;

        // disable NavMeshAgent
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        // disable collider
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // start post-death sequences
        StartCoroutine(ShowDialogueAfterDelay(3f));
        StartCoroutine(RemoveRocksAfterDelay(4f));
    }

    private IEnumerator ShowDialogueAfterDelay(float delay)
    {
        // wait then start dialogue via DialogueManager
        yield return new WaitForSeconds(delay);

        DialogueManager dm = FindObjectOfType<DialogueManager>();
        if (dm != null && bossDeathDialogue != null && bossDeathDialogue.Length > 0)
        {
            dm.StartDialogue(bossDeathDialogue);
        }
    }

    private IEnumerator RemoveRocksAfterDelay(float delay)
    {
        // wait then remove blocking rocks
        yield return new WaitForSeconds(delay);
        RemoveBlockingRocks();
    }

    private void RemoveBlockingRocks()
    {
        // disable each assigned rock
        if (blockingRocks != null && blockingRocks.Length > 0)
        {
            foreach (GameObject rock in blockingRocks)
            {
                if (rock != null)
                {
                    rock.SetActive(false);
                }
            }
        }
    }
}
