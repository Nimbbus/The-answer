using UnityEngine;
using UnityEngine.AI;

public class RageAI : MonoBehaviour
{
    public Transform player;          // Assign the player in Inspector
    public float attackRange = 2f;    // Distance at which boss attacks
    public float attackCooldown = 3f; // Idle time between attacks
    public float attackDuration = 1f; // Length of attack animation
    public float rotationSpeed = 5f;  // Speed at which boss rotates to face player
    public int attackDamage = 10;     // Damage dealt to player per hit

    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange && !isAttacking)
        {
            // ✅ Chase player
            if (agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }

            FacePlayerSmooth(); // Rotate toward player while chasing

            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            // ✅ Attack logic
            if (!isAttacking)
            {
                if (agent.isOnNavMesh)
                {
                    agent.isStopped = true;
                }

                animator.SetBool("isWalking", false);
                StartCoroutine(AttackRoutine());
            }
        }
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // ✅ Snap to face player before attack
        FacePlayerInstant();

        animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(attackDuration);

        animator.SetBool("isAttacking", false);

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    // ✅ Called via Animation Event at the moment of impact
    public void DealDamage()
    {
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
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
