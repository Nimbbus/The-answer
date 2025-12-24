using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class DespairAI : MonoBehaviour
{
    [Header("References")]
    public Transform player; // player transform to follow/face

    [Header("Ranges")]
    public float chaseRange = 10f; // start chasing if within this
    public float attackRange = 5f; // start attacking if within this
    public float stopDistance = 4.5f; // NavMeshAgent stopping distance

    [Header("Timing")]
    public float attackCooldown = 2f; // delay between attack sequences
    public float hitRecoveryTime = 0.1f; // brief recovery after being hit

    [Header("Attack Effects")]
    public GameObject firstAttackEffectPrefab; // effect for first attack
    public GameObject secondAttackEffectPrefab; // effect for second attack
    public Transform leftHand; // spawn point for left effect
    public Transform rightHand; // spawn point for right effect
    public float effectLifetime = 2f; // how long spawned effects live

    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking;
    private bool isDead;
    private bool isOnCooldown;
    private bool isRecovering;

    void Start()
    {
        // cache components and set stopping distance
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = stopDistance;
    }

    void Update()
    {
        // don't act if no player, dead or recovering
        if (player == null || isDead || isRecovering) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // attack when in range
        if (distance <= attackRange)
        {
            if (!isAttacking && !isOnCooldown)
            {
                StartCoroutine(AttackOnce());
            }
        }
        // chase when in chase range
        else if (distance <= chaseRange)
        {
            ChasePlayer();
        }
        // idle otherwise
        else
        {
            animator.SetBool("isWalking", false);
            agent.isStopped = true;
        }

        // keep facing player while attacking
        if (isAttacking)
        {
            FacePlayer();
        }
    }

    void ChasePlayer()
    {
        if (isDead) return;
        // move toward player and play walk animation
        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator.SetBool("isWalking", true);
    }

    IEnumerator AttackOnce()
    {
        if (isRecovering || isDead) yield break;

        // begin attack sequence
        isAttacking = true;
        agent.isStopped = true;
        animator.SetBool("isWalking", false);
        animator.speed = 2.0f;

        // play first attack and effect
        animator.SetTrigger("FirstAttack");
        SpawnAttackEffect(firstAttackEffectPrefab);
        yield return AttackMove(0.5f);

        if (isRecovering || isDead) yield break;

        // play second attack and effect
        animator.SetTrigger("SecAttack");
        SpawnAttackEffect(secondAttackEffectPrefab);
        yield return AttackMove(0.5f);

        // restore normal speed and resume chase
        animator.speed = 1.0f;
        isAttacking = false;

        if (!isDead)
        {
            agent.isStopped = false;
            animator.SetTrigger("ResumeChase");
        }

        // cooldown before next attack
        isOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false;
    }

    void SpawnAttackEffect(GameObject prefab)
    {
        if (prefab == null) return;

        if (leftHand != null)
        {
            GameObject effectL = Instantiate(prefab, leftHand.position, leftHand.rotation);
            Destroy(effectL, effectLifetime);
        }

        if (rightHand != null)
        {
            GameObject effectR = Instantiate(prefab, rightHand.position, rightHand.rotation);
            Destroy(effectR, effectLifetime);
        }
    }

    IEnumerator AttackMove(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (isRecovering || isDead) yield break;

            // face player and step forward if too far
            FacePlayer();

            float dist = Vector3.Distance(transform.position, player.position);
            if (dist > agent.stoppingDistance)
            {
                Vector3 dir = (player.position - transform.position).normalized;
                agent.Move(dir * 1.5f * Time.deltaTime); // small forward movement during attack
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    void FacePlayer()
    {
        if (isDead) return;
        // rotate smoothly to look at player on the horizontal plane
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

        // react to being hit and stop current actions
        animator.SetTrigger("GettingHit");
        StopAllCoroutines();

        isAttacking = false;
        isOnCooldown = false;
        agent.isStopped = true;

        // start brief recovery if not already recovering
        if (!isRecovering)
        {
            StartCoroutine(RecoverAfterHit());
        }
    }

    IEnumerator RecoverAfterHit()
    {
        // short invulnerable/recovery period
        isRecovering = true;
        yield return new WaitForSeconds(hitRecoveryTime);
        isRecovering = false;

        // resume chasing after recovery
        if (!isDead)
        {
            agent.isStopped = false;
            animator.SetTrigger("ResumeChase");
        }
    }

    public void OnDeath()
    {
        if (isDead) return;
        // mark dead and disable movement
        isDead = true;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.enabled = false;
        }

        // play death animation and stop walking
        animator.SetTrigger("Die");
        animator.SetBool("isWalking", false);
    }
}
