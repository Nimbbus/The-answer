using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class DespairAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Ranges")]
    public float chaseRange = 10f;
    public float attackRange = 5f;
    public float stopDistance = 4.5f;

    [Header("Timing")]
    public float attackCooldown = 2f;
    public float hitRecoveryTime = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking;
    private bool isDead;
    private bool isOnCooldown;
    private bool isRecovering;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = stopDistance;
    }

    void Update()
    {
        if (player == null || isDead || isRecovering) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            if (!isAttacking && !isOnCooldown)
            {
                StartCoroutine(AttackOnce());
            }
        }
        else if (distance <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            animator.SetBool("isWalking", false);
            agent.isStopped = true;
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

    IEnumerator AttackOnce()
    {
        if (isRecovering || isDead) yield break;

        isAttacking = true;
        agent.isStopped = true;
        animator.SetBool("isWalking", false);
        animator.speed = 2.0f;

        animator.SetTrigger("FirstAttack");
        yield return AttackMove(0.5f);

        if (isRecovering || isDead) yield break;

        animator.SetTrigger("SecAttack");
        yield return AttackMove(0.5f);

        animator.speed = 1.0f;
        isAttacking = false;

        if (!isDead)
        {
            agent.isStopped = false;
            animator.SetTrigger("ResumeChase");
        }

        isOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false;
    }

    IEnumerator AttackMove(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (isRecovering || isDead) yield break;

            FacePlayer();

            float dist = Vector3.Distance(transform.position, player.position);
            if (dist > agent.stoppingDistance)
            {
                Vector3 dir = (player.position - transform.position).normalized;
                agent.Move(dir * 1.5f * Time.deltaTime);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
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
            animator.SetTrigger("ResumeChase");
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
}
