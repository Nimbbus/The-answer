using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // pause UI panel
    public bool isPaused = false; // whether game is paused

    void Update()
    {
        // toggle pause on Escape or P
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

        // clear UI selection while paused to avoid stuck focus
        if (isPaused)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    // switch between paused and resumed states
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // enable pause UI and freeze time
    private void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // hide pause UI and resume time
    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // go back to main menu (resumes time first)
    public void GoToMainMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // quit the application (ensure timeScale restored)
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
