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
    public Animator animator; // Assign character's Animator in Inspector

    public float lightAttackCooldown = 0.5f;
    public float heavyAttackCooldown = 1.0f;

    private float lastLightAttackTime;
    private float lastHeavyAttackTime;
    private bool isAttacking;

    private CharacterController controller;
    private float verticalVelocity;
    public float gravity = -9.81f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Auto-assign Animator if not set
        if (!animator) animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        ApplyGravity();
        HandleDodge();
        HandleAttack();
    }

    void HandleMovement()
    {
        if (isAttacking) return; // Lock movement during attack/dodge

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
        lastDodgeTime = Time.time;
        isAttacking = true; // lock movement during dodge

        if (animator) animator.SetTrigger("Dodge");

        float dodgeDuration = 0.3f; // match animation length
        float elapsed = 0f;
        Vector3 dodgeDir = transform.forward;

        while (elapsed < dodgeDuration)
        {
            controller.Move(dodgeDir * (dodgeDistance / dodgeDuration) * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isAttacking = false;
    }

    void HandleAttack()
    {
        // Light Attack (Left Click)
        if (Input.GetMouseButtonDown(0) && Time.time > lastLightAttackTime + lightAttackCooldown && !isAttacking)
        {
            StartCoroutine(PerformAttack("LightAttack", lightAttackCooldown));
        }

        // Heavy Attack (Right Click)
        if (Input.GetMouseButtonDown(1) && Time.time > lastHeavyAttackTime + heavyAttackCooldown && !isAttacking)
        {
            StartCoroutine(PerformAttack("HeavyAttack", heavyAttackCooldown));
        }
    }

    IEnumerator PerformAttack(string attackTrigger, float cooldown)
    {
        isAttacking = true;

        if (attackTrigger == "LightAttack")
            lastLightAttackTime = Time.time;
        else if (attackTrigger == "HeavyAttack")
            lastHeavyAttackTime = Time.time;

        if (animator) animator.SetTrigger(attackTrigger);

        // Lock movement for duration of animation
        yield return new WaitForSeconds(cooldown);
        isAttacking = false;
    }
}
