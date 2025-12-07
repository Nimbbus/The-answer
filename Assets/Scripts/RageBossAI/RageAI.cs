using UnityEngine;
using UnityEngine.AI;

public class RageAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Collider weaponCollider;

    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 3f;
    public float rotationSpeed = 5f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (weaponCollider != null)
        {
            weaponCollider.isTrigger = true;
            weaponCollider.enabled = false;
        }
    }

    void Update()
    {
        // ✅ Stop all AI actions if player is dead
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

        FacePlayerInstant();
        animator.SetBool("isAttacking", true);

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

        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    public void EnableWeaponCollider()
    {
        if (weaponCollider != null && !PlayerHealth.IsPlayerDead)
        {
            weaponCollider.enabled = true;

            WeaponHit hitScript = weaponCollider.GetComponent<WeaponHit>();
            if (hitScript != null) hitScript.ResetHit();

            Debug.Log("Weapon collider enabled (via Animation Event).");
        }
    }

    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
            Debug.Log("Weapon collider disabled (via Animation Event).");
        }
    }

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
