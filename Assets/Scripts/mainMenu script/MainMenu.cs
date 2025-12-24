using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI components

// Controls main menu UI and audio
public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel; // settings UI panel
    public GameObject aboutPanel;    // about UI panel

    [Header("Audio")]
    public Slider volumeSlider;      // UI slider for volume
    private const float maxSafeVolume = 0.5f; // safe max volume (50%)

    void Start()
    {
        // set default safe volume
        AudioListener.volume = maxSafeVolume;

        if (volumeSlider != null)
        {
            // initialize slider and subscribe callback
            volumeSlider.value = 1f; // slider shown as max
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    // start the main game scene
    public void StartGame()
    {
        SceneManager.LoadScene("OldManMeeting");
    }

    // show settings panel
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // hide settings panel
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    // quit the application
    public void QuitGame()
    {
        Application.Quit();
    }

    // show about panel
    public void OpenAbout()
    {
        aboutPanel.SetActive(true);
    }

    // hide about panel
    public void CloseAbout()
    {
        aboutPanel.SetActive(false);
    }

    // map slider [0..1] to AudioListener.volume [0..maxSafeVolume]
    public void SetVolume(float value)
    {
        AudioListener.volume = Mathf.Lerp(0f, maxSafeVolume, Mathf.Clamp01(value));
    }
}
