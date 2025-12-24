using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    [Header("References")]
    public GameObject healthCanvas;          // Canvas that contains health UI
    public DespairSceneHealth playerHealth;  // Player health component
    public DespairSceneHealth bossHealth;    // Boss health component

    private Slider playerSlider; // player health slider
    private Slider bossSlider;   // boss health slider
    private Text playerText;     // player health text
    private Text bossText;       // boss health text

    void Awake()
    {
        // find slider objects under the canvas
        if (healthCanvas == null)
        {
            // no canvas assigned
            return;
        }

        Transform playerSliderObj = healthCanvas.transform.Find("PlayerSlider");
        Transform bossSliderObj = healthCanvas.transform.Find("BossSlider");

        if (playerSliderObj != null)
            playerSlider = playerSliderObj.GetComponent<Slider>();
        else
            // player slider not found
            playerSlider = null;

        if (bossSliderObj != null)
            bossSlider = bossSliderObj.GetComponent<Slider>();
        else
            // boss slider not found
            bossSlider = null;

        Transform playerTextObj = playerSliderObj?.Find("PlayerText");
        Transform bossTextObj = bossSliderObj?.Find("BossText");

        if (playerTextObj != null)
            playerText = playerTextObj.GetComponent<Text>();
        else
            // player text not found
            playerText = null;

        if (bossTextObj != null)
            bossText = bossTextObj.GetComponent<Text>();
        else
            // boss text not found
            bossText = null;
    }

    void Start()
    {
        // initialize slider ranges and values
        if (playerHealth != null && playerSlider != null)
        {
            playerSlider.maxValue = playerHealth.maxHealth;
            playerSlider.value = playerHealth.currentHealth;
        }

        if (bossHealth != null && bossSlider != null)
        {
            bossSlider.maxValue = bossHealth.maxHealth;
            bossSlider.value = bossHealth.currentHealth;
        }

        UpdateTexts();
    }

    void Update()
    {
        // update slider values each frame
        if (playerHealth != null && playerSlider != null)
            playerSlider.value = playerHealth.currentHealth;

        if (bossHealth != null && bossSlider != null)
            bossSlider.value = bossHealth.currentHealth;

        UpdateTexts();
    }

    void UpdateTexts()
    {
        // update numeric health displays
        if (playerText != null && playerHealth != null)
            playerText.text = playerHealth.currentHealth + " / " + playerHealth.maxHealth;

        if (bossText != null && bossHealth != null)
            bossText.text = bossHealth.currentHealth + " / " + bossHealth.maxHealth;
    }
}