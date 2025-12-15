using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    [Header("References")]
    public GameObject healthCanvas;          // Drag the whole Canvas here
    public DespairSceneHealth playerHealth;  // Drag Player GameObject here
    public DespairSceneHealth bossHealth;    // Drag Boss GameObject here

    private Slider playerSlider;
    private Slider bossSlider;
    private Text playerText;
    private Text bossText;

    void Awake()
    {
        if (healthCanvas == null)
        {
            Debug.LogError("HealthCanvas is not assigned.");
            return;
        }

        Transform playerSliderObj = healthCanvas.transform.Find("PlayerSlider");
        Transform bossSliderObj = healthCanvas.transform.Find("BossSlider");

        if (playerSliderObj != null)
            playerSlider = playerSliderObj.GetComponent<Slider>();
        else
            Debug.LogError("PlayerSlider not found under HealthCanvas.");

        if (bossSliderObj != null)
            bossSlider = bossSliderObj.GetComponent<Slider>();
        else
            Debug.LogError("BossSlider not found under HealthCanvas.");

        Transform playerTextObj = playerSliderObj?.Find("PlayerText");
        Transform bossTextObj = bossSliderObj?.Find("BossText");

        if (playerTextObj != null)
            playerText = playerTextObj.GetComponent<Text>();
        else
            Debug.LogWarning("PlayerText not found under PlayerSlider.");

        if (bossTextObj != null)
            bossText = bossTextObj.GetComponent<Text>();
        else
            Debug.LogWarning("BossText not found under BossSlider.");
    }

    void Start()
    {
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
        if (playerHealth != null && playerSlider != null)
            playerSlider.value = playerHealth.currentHealth;

        if (bossHealth != null && bossSlider != null)
            bossSlider.value = bossHealth.currentHealth;

        UpdateTexts();
    }

    void UpdateTexts()
    {
        if (playerText != null && playerHealth != null)
            playerText.text = playerHealth.currentHealth + " / " + playerHealth.maxHealth;

        if (bossText != null && bossHealth != null)
            bossText.text = bossHealth.currentHealth + " / " + bossHealth.maxHealth;
    }
}
