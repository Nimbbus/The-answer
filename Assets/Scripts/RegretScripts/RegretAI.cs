using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class RegretAI : MonoBehaviour
{
    [Header("References")]
    public Transform player; // player transform

    [Header("Ranges")]
    public float chaseRange = 12f;       // start chasing within this
    public float attackRange = 4f;       // start attacking within this
    public float stopDistance = 3.5f;    // nav agent stopping distance

    [Header("Timing")]
    public float attackCooldown = 2f;    // cooldown after attack sequence
    public float hitRecoveryTime = 1.0f; // recovery time after being hit

    [Header("Weapon")]
    public Collider weaponCollider;      // weapon collider (enable during attack)
    public Transform swordTransform;     // where to spawn attached effects
    public int firstAttackDamage = 25;   // damage for first attack
    public int secondAttackDamage = 40;  // damage for second attack

    [Header("Attack Effects")]
    public GameObject firstAttackEffectPrefab;   // visual effect prefab for first attack
    public GameObject secondAttackEffectPrefab;  // visual effect prefab for second attack
    public float effectLifetime = 2f;            // lifetime of spawned effects

    private NavMeshAgent agent;         // NavMeshAgent reference
    private Animator animator;          // Animator reference
    private RegretBossHit hitScript;    // script that applies hit logic

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

        // prepare weapon collider and hit script
        if (weaponCollider != null)
        {
            hitScript = weaponCollider.GetComponent<RegretBossHit>();
            weaponCollider.enabled = false; // disabled until attack frames
        }

        // start in walking state
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        // do nothing if no player, dead, or recovering
        if (player == null || isDead || isRecovering) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // begin attack sequence if in range and available
        if (distance <= attackRange && !isAttacking && !isOnCooldown)
        {
            StartCoroutine(AttackSequence());
        }
        // chase player if within chase range
        else if (distance <= chaseRange)
        {
            ChasePlayer();
        }
        // idle otherwise
        else
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
        }

        // face player while attacking
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
        // begin attack sequence
        isAttacking = true;
        agent.isStopped = true;
        animator.SetBool("isWalking", false);

        // first attack
        animator.SetTrigger("FirstAttack");
        SpawnAttackEffect(firstAttackEffectPrefab);
        yield return new WaitForSeconds(3.8f); // wait matching first attack anim

        // second attack follows
        animator.SetTrigger("SecAttack");
        SpawnAttackEffect(secondAttackEffectPrefab);
        yield return new WaitForSeconds(1.2f); // wait matching second attack anim

        // finish sequence and resume chase
        isAttacking = false;
        agent.isStopped = false;
        animator.SetBool("isWalking", true);

        // start cooldown after sequence
        isOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false;
    }

    void SpawnAttackEffect(GameObject prefab)
    {
        // spawn effect as child of the sword so it follows the swing
        if (prefab == null || swordTransform == null) return;

        GameObject effectInstance = Instantiate(prefab, swordTransform);
        effectInstance.transform.localPosition = Vector3.zero;
        effectInstance.transform.localRotation = Quaternion.identity;

        Destroy(effectInstance, effectLifetime);
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

        // play hit animation and stop current actions
        animator.SetTrigger("GettingHit");
        StopAllCoroutines();

        isAttacking = false;
        isOnCooldown = false;
        agent.isStopped = true;

        // start recovery if not already recovering
        if (!isRecovering)
        {
            StartCoroutine(RecoverAfterHit());
        }
    }

    IEnumerator RecoverAfterHit()
    {
        // short recovery period
        isRecovering = true;
        yield return new WaitForSeconds(hitRecoveryTime);
        isRecovering = false;

        if (!isDead)
        {
            agent.isStopped = false;
            animator.SetBool("isWalking", true);

            // optionally resume attack if still in range
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

        // stop and disable navigation
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

    // enable collider for first attack and set damage
    public void EnableFirstAttackCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            if (hitScript != null)
            {
                hitScript.ResetHit(); // allow one hit this swing
                hitScript.SetDamage(firstAttackDamage);
            }
        }
    }

    // enable collider for second attack and set damage
    public void EnableSecondAttackCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            if (hitScript != null)
            {
                hitScript.ResetHit(); // allow one hit this swing
                hitScript.SetDamage(secondAttackDamage);
            }
        }
    }

    // disable weapon collider outside attack frames
    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }
}
