using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class RegretAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Ranges")]
    public float chaseRange = 12f;       // how far boss will chase
    public float attackRange = 4f;       // how close before attacking
    public float stopDistance = 3.5f;    // stopping distance before swinging

    [Header("Timing")]
    public float attackCooldown = 2f;    // cooldown AFTER second attack
    public float hitRecoveryTime = 1.0f; // recovery after getting hit (adjust in Inspector)

    [Header("Weapon")]
    public Collider weaponCollider; // assign boss weapon collider in Inspector
    public int firstAttackDamage = 25;
    public int secondAttackDamage = 40;

    private NavMeshAgent agent;
    private Animator animator;
    private RegretBossHit hitScript;

    private bool isAttacking;
    private bool isDead;
    private bool isOnCooldown;
    private bool isRecovering;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = stopDistance;

        if (weaponCollider != null)
        {
            hitScript = weaponCollider.GetComponent<RegretBossHit>();
            weaponCollider.enabled = false; // keep disabled until attack frames
        }

        // Regret always starts walking
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        if (player == null || isDead || isRecovering) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && !isAttacking && !isOnCooldown)
        {
            StartCoroutine(AttackSequence());
        }
        else if (distance <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
        }

        if (isAttacking)
        {
            FacePlayer();
        }
    }

    void ChasePlayer()
    {
        if (isDead) return;
        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator.SetBool("isWalking", true);
    }

    IEnumerator AttackSequence()
    {
        isAttacking = true;
        agent.isStopped = true;
        animator.SetBool("isWalking", false);

        // First attack
        animator.SetTrigger("FirstAttack");
        yield return new WaitForSeconds(0.7f); // adjust to match animation length

        // Second attack immediately after
        animator.SetTrigger("SecAttack");
        yield return new WaitForSeconds(0.7f);

        // End of attack sequence
        isAttacking = false;
        agent.isStopped = false;
        animator.SetBool("isWalking", true);

        // ✅ Cooldown starts AFTER SecAttack finishes
        isOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false;
    }

    void FacePlayer()
    {
        if (isDead) return;
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    public void OnHit()
    {
        if (isDead) return;

        animator.SetTrigger("GettingHit");
        StopAllCoroutines();

        isAttacking = false;
        isOnCooldown = false;
        agent.isStopped = true;

        if (!isRecovering)
        {
            StartCoroutine(RecoverAfterHit());
        }
    }

    IEnumerator RecoverAfterHit()
    {
        isRecovering = true;
        yield return new WaitForSeconds(hitRecoveryTime);
        isRecovering = false;

        if (!isDead)
        {
            agent.isStopped = false;
            animator.SetBool("isWalking", true);

            // Optional: immediately attack if player is still in range
            if (Vector3.Distance(transform.position, player.position) <= attackRange && !isOnCooldown)
            {
                StartCoroutine(AttackSequence());
            }
        }
    }

    public void OnDeath()
    {
        if (isDead) return;
        isDead = true;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.enabled = false;
        }

        animator.SetTrigger("Die");
        animator.SetBool("isWalking", false);
    }

    // ✅ Animation Event methods
    public void EnableFirstAttackCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            if (hitScript != null)
            {
                hitScript.ResetHit(); // ensures only one hit per swing
                hitScript.SetDamage(firstAttackDamage);
            }
        }
    }

    public void EnableSecondAttackCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            if (hitScript != null)
            {
                hitScript.ResetHit(); // ensures only one hit per swing
                hitScript.SetDamage(secondAttackDamage);
            }
        }
    }

    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }
}
