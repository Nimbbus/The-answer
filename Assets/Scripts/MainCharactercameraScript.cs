using UnityEngine;

public class MainCharactercameraScript : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player;          // Assign your player here
    public Vector3 offset = new Vector3(0, 2, -4); // Default offset (height + distance)

    [Header("Camera Settings")]
    public float rotationSpeed = 5f;  // Mouse sensitivity
    public float smoothSpeed = 10f;   // How smoothly the camera follows

    private float yaw;                // Horizontal rotation
    private float pitch;              // Vertical rotation

    void LateUpdate()
    {
        if (!player) return;

        // Get mouse input
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -20f, 60f); // Clamp vertical angle

        // Calculate rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Desired position behind the player
        Vector3 desiredPosition = player.position + rotation * offset;

        // Smoothly move camera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Always look at the player
        transform.LookAt(player.position + Vector3.up * 1.5f); // Aim at chest/head height
    }
}
