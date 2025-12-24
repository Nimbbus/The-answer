using UnityEngine;
using UnityEngine.AI;

public class RageAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;               // player transform to follow
    public Collider weaponCollider;        // weapon collider (enabled during attacks)

    [Header("Attack Settings")]
    public float attackRange = 2f;         // distance to start attacking
    public float attackCooldown = 3f;      // time between attacks
    public float rotationSpeed = 5f;       // smooth rotation speed

    [Header("Attack Effect")]
    public GameObject attackEffectPrefab;  // effect spawned on attack
    public Transform effectSpawnPoint;     // where to spawn effect
    public float effectLifetime = 2f;      // lifetime of spawned effect

    private NavMeshAgent agent;            // navmesh agent for movement
    private Animator animator;             // animator reference
    private bool isAttacking = false;      // whether currently in attack routine

    void Start()
    {
        // cache components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // prepare weapon collider as trigger, start disabled
        if (weaponCollider != null)
        {
            weaponCollider.isTrigger = true;
            weaponCollider.enabled = false;
        }
    }

    void Update()
    {
        // stop behavior if player is dead
        if (PlayerHealth.IsPlayerDead)
        {
            if (agent.isOnNavMesh) agent.isStopped = true;

            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);

            isAttacking = false;
            if (weaponCollider != null) weaponCollider.enabled = false;

            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        // chase when out of attack range
        if (distance > attackRange && !isAttacking)
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }

            FacePlayerSmooth();

            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            // start attack when in range
            if (!isAttacking)
            {
                if (agent.isOnNavMesh) agent.isStopped = true;

                animator.SetBool("isWalking", false);
                StartCoroutine(AttackRoutine());
            }
        }
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        if (PlayerHealth.IsPlayerDead) yield break;

        isAttacking = true;

        // snap to face player at attack start
        FacePlayerInstant();
        animator.SetBool("isAttacking", true);

        // spawn visual attack effect
        if (attackEffectPrefab != null)
        {
            Transform spawnPoint = effectSpawnPoint != null ? effectSpawnPoint : transform;
            GameObject effectInstance = Instantiate(attackEffectPrefab, spawnPoint.position, spawnPoint.rotation);

            // destroy effect after lifetime
            Destroy(effectInstance, effectLifetime);
        }

        // wait while attack cooldown elapses
        float elapsed = 0f;
        while (elapsed < attackCooldown)
        {
            if (PlayerHealth.IsPlayerDead)
            {
                animator.SetBool("isAttacking", false);
                isAttacking = false;
                if (weaponCollider != null) weaponCollider.enabled = false;
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // end attack state
        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    // enable weapon collider to register hits
    public void EnableWeaponCollider()
    {
        if (weaponCollider != null && !PlayerHealth.IsPlayerDead)
        {
            weaponCollider.enabled = true;

            WeaponHit hitScript = weaponCollider.GetComponent<WeaponHit>();
            if (hitScript != null) hitScript.ResetHit();
        }
    }

    // disable weapon collider
    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }

    // smooth rotation toward player
    private void FacePlayerSmooth()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    // instant rotation to face player
    private void FacePlayerInstant()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        }
    }
}
