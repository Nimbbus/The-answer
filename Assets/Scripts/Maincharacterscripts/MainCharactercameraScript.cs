using UnityEngine;

public class MainCharactercameraScript : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player;               // object camera follows
    public Vector3 offset = new Vector3(0, 2, -4); // camera offset from player

    [Header("Camera Settings")]
    public float rotationSpeed = 5f;       // mouse rotation sensitivity
    public float smoothSpeed = 10f;        // position smoothing speed

    private float yaw;                     // horizontal angle
    private float pitch;                   // vertical angle

    private PauseMenu pauseMenu;           // reference to pause menu

    void Start()
    {
        // cache pause menu if present
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    void LateUpdate()
    {
        if (!player) return; // need a player to follow

        // don't rotate camera while game is paused
        if (pauseMenu != null && pauseMenu.isPaused)
            return;

        // read mouse input
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -20f, 60f); // limit vertical angle

        // build rotation from angles
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // compute desired camera position using rotation and offset
        Vector3 desiredPosition = player.position + rotation * offset;

        // move camera smoothly to desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // always look toward player head area
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}
