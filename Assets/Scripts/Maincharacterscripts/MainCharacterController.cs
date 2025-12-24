using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class MainCharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;           // movement speed
    public float rotationSpeed = 720f;     // rotation speed (deg/sec)

    [Header("Dodge")]
    public float dodgeDistance = 5f;       // distance covered by dodge
    public float dodgeCooldown = 1f;       // time between dodges
    private float lastDodgeTime;           // timestamp of last dodge

    [Header("Attack")]
    public Animator animator;              // animator reference

    public float lightAttackCooldown = 1.5f;    // light attack cooldown
    public float heavyAttackCooldown = 1.7f;    // heavy attack cooldown
    public float lightAttackLockDuration = 0.6f;// input lock during light attack
    public float heavyAttackLockDuration = 1.0f;// input lock during heavy attack

    private float lastLightAttackTime;     // last light attack time
    private float lastHeavyAttackTime;     // last heavy attack time
    private bool isAttacking;              // input lock while attacking

    private CharacterController controller; // character controller component
    private float verticalVelocity;        // vertical speed for gravity
    public float gravity = -9.81f;         // gravity value

    public bool isDead = false;            // set true when player dies

    [Header("Audio Clips")]
    public AudioClip lightAttackClip;      // sound for light attack
    public AudioClip heavyAttackClip;      // sound for heavy attack

    private AudioSource audioSource;       // cached audio source

    void Start()
    {
        // cache components and lock cursor
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (!animator) animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return; // no input when dead

        HandleMovement();
        ApplyGravity();
        HandleDodge();
        HandleAttack();
    }

    void HandleMovement()
    {
        if (isAttacking) return; // don't move while attacking

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v).normalized;
        bool isWalking = inputDir.magnitude >= 0.1f;

        if (animator) animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            // rotate to face movement direction relative to camera
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            // move forward
            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir * moveSpeed * Time.deltaTime);
        }
    }

    void ApplyGravity()
    {
        if (isDead) return; // stop physics after death

        if (!controller.isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime; // accumulate fall speed
        }
        else
        {
            verticalVelocity = -1f; // keep grounded
        }

        Vector3 gravityMove = new Vector3(0, verticalVelocity, 0);
        controller.Move(gravityMove * Time.deltaTime);
    }

    void HandleDodge()
    {
        // start dodge if LeftShift pressed and cooldown passed
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDodgeTime + dodgeCooldown && !isAttacking)
        {
            StartCoroutine(PerformDodge());
        }
    }

    IEnumerator PerformDodge()
    {
        if (isDead) yield break; // cancel if dead

        lastDodgeTime = Time.time;
        isAttacking = true;

        if (animator) animator.SetTrigger("Dodge");

        float dodgeDuration = 0.3f;
        float elapsed = 0f;
        Vector3 dodgeDir = transform.forward;

        // move forward over dodgeDuration
        while (elapsed < dodgeDuration)
        {
            if (isDead) yield break; // stop if died mid-dodge

            controller.Move(dodgeDir * (dodgeDistance / dodgeDuration) * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (!isDead) isAttacking = false; // release input lock
    }

    void HandleAttack()
    {
        if (isDead) return; // no attacks when dead

        // Light Attack input
        if (Input.GetMouseButtonDown(0) && Time.time > lastLightAttackTime + lightAttackCooldown && !isAttacking)
        {
            StartCoroutine(PerformAttack("LightAttack", lightAttackLockDuration, lightAttackClip));
            lastLightAttackTime = Time.time;
        }

        // Heavy Attack input
        if (Input.GetMouseButtonDown(1) && Time.time > lastHeavyAttackTime + heavyAttackCooldown && !isAttacking)
        {
            StartCoroutine(PerformAttack("HeavyAttack", heavyAttackLockDuration, heavyAttackClip));
            lastHeavyAttackTime = Time.time;
        }
    }

    IEnumerator PerformAttack(string attackTrigger, float lockDuration, AudioClip clip)
    {
        if (isDead) yield break; // cancel if dead

        isAttacking = true;
        if (animator) animator.SetTrigger(attackTrigger);

        // play sound if available
        if (clip && audioSource) audioSource.PlayOneShot(clip);

        // lock inputs for attack duration
        yield return new WaitForSeconds(lockDuration);

        if (!isDead) isAttacking = false; // release lock
    }
}
