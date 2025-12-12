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

    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking;
    private bool isDead;
    private bool isOnCooldown;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = stopDistance;
    }

    void Update()
    {
        if (player == null || isDead) return;

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
        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator.SetBool("isWalking", true);
    }

    IEnumerator AttackOnce()
    {
        isAttacking = true;
        agent.isStopped = true;
        animator.SetBool("isWalking", false);

        animator.SetTrigger("FirstAttack");
        yield return AttackMove(1.0f);

        animator.SetTrigger("SecAttack");
        yield return AttackMove(1.0f);

        isAttacking = false;
        agent.isStopped = false;
        animator.SetTrigger("ResumeChase");

        isOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false;
    }

    IEnumerator AttackMove(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
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
        animator.SetTrigger("GettingHit");
        isAttacking = false;
        agent.isStopped = false;
        animator.SetTrigger("ResumeChase");
    }

    public void OnDeath()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        animator.SetBool("isWalking", false);
    }
}
