using UnityEngine;

public class MainCharactercameraScript : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player;
    public Vector3 offset = new Vector3(0, 2, -4);

    [Header("Camera Settings")]
    public float rotationSpeed = 5f;
    public float smoothSpeed = 10f;

    private float yaw;
    private float pitch;

    private PauseMenu pauseMenu; // Reference to pause menu

    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    void LateUpdate()
    {
        if (!player) return;

        // ✅ Skip camera rotation if paused
        if (pauseMenu != null && pauseMenu.isPaused)
            return;

        // Get mouse input
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -20f, 60f);

        // Calculate rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Desired position behind the player
        Vector3 desiredPosition = player.position + rotation * offset;

        // Smoothly move camera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Always look at the player
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}
