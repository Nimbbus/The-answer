using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class MainCharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

    [Header("Dodge")]
    public float dodgeDistance = 5f;
    public float dodgeCooldown = 1f;
    private float lastDodgeTime;

    [Header("Attack")]
    public Animator animator;

    public float lightAttackCooldown = 1.5f;
    public float heavyAttackCooldown = 1.7f;
    public float lightAttackLockDuration = 0.6f;
    public float heavyAttackLockDuration = 1.0f;

    private float lastLightAttackTime;
    private float lastHeavyAttackTime;
    private bool isAttacking;

    private CharacterController controller;
    private float verticalVelocity;
    public float gravity = -9.81f;

    public bool isDead = false; // flag set by DespairSceneHealth on death

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (!animator) animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return; // stop all input if dead

        HandleMovement();
        ApplyGravity();
        HandleDodge();
        HandleAttack();
    }

    void HandleMovement()
    {
        if (isAttacking) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v).normalized;
        bool isWalking = inputDir.magnitude >= 0.1f;

        if (animator) animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir * moveSpeed * Time.deltaTime);
        }
    }

    void ApplyGravity()
    {
        if (isDead) return; // prevent gravity after death

        if (!controller.isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else
        {
            verticalVelocity = -1f;
        }

        Vector3 gravityMove = new Vector3(0, verticalVelocity, 0);
        controller.Move(gravityMove * Time.deltaTime);
    }

    void HandleDodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDodgeTime + dodgeCooldown && !isAttacking)
        {
            StartCoroutine(PerformDodge());
        }
    }

    IEnumerator PerformDodge()
    {
        if (isDead) yield break; // prevent dodge if dead

        lastDodgeTime = Time.time;
        isAttacking = true;

        if (animator) animator.SetTrigger("Dodge");

        float dodgeDuration = 0.3f;
        float elapsed = 0f;
        Vector3 dodgeDir = transform.forward;

        while (elapsed < dodgeDuration)
        {
            if (isDead) yield break; // stop mid‑dodge if dead

            controller.Move(dodgeDir * (dodgeDistance / dodgeDuration) * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (!isDead) isAttacking = false;
    }

    void HandleAttack()
    {
        if (isDead) return; // prevent attacks after death

        // Light Attack
        if (Input.GetMouseButtonDown(0) && Time.time > lastLightAttackTime + lightAttackCooldown && !isAttacking)
        {
            StartCoroutine(PerformAttack("LightAttack", lightAttackLockDuration));
            lastLightAttackTime = Time.time;
        }

        // Heavy Attack
        if (Input.GetMouseButtonDown(1) && Time.time > lastHeavyAttackTime + heavyAttackCooldown && !isAttacking)
        {
            StartCoroutine(PerformAttack("HeavyAttack", heavyAttackLockDuration));
            lastHeavyAttackTime = Time.time;
        }
    }

    IEnumerator PerformAttack(string attackTrigger, float lockDuration)
    {
        if (isDead) yield break; // prevent attack if dead

        isAttacking = true;
        if (animator) animator.SetTrigger(attackTrigger);

        yield return new WaitForSeconds(lockDuration);

        if (!isDead) isAttacking = false;
    }
}
