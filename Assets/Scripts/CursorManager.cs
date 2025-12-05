using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        string scene = SceneManager.GetActiveScene().name;

        // ✅ Always visible in MainMenu
        if (scene == "MainMenu")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        // ✅ Visible if paused
        PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu != null && pauseMenu.isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        // ✅ Default gameplay: cursor hidden
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
