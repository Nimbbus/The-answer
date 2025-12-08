using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    public Slider playerHealthBar;
    public Slider bossHealthBar;

    private PlayerHealth playerHealth;
    private BossHealth bossHealth;

    void Start()
    {
        // Find references in scene
        playerHealth = FindObjectOfType<PlayerHealth>();
        bossHealth = FindObjectOfType<BossHealth>();

        if (playerHealthBar != null)
        {
            playerHealthBar.maxValue = playerHealth.maxHealth;
            playerHealthBar.value = playerHealth.CurrentHealth;
        }

        if (bossHealthBar != null)
        {
            bossHealthBar.maxValue = bossHealth.maxHealth;
            bossHealthBar.value = bossHealth.CurrentHealth;
        }
    }

    void Update()
    {
        if (playerHealth != null && playerHealthBar != null)
        {
            playerHealthBar.value = playerHealth.CurrentHealth;
        }

        if (bossHealth != null && bossHealthBar != null)
        {
            bossHealthBar.value = bossHealth.CurrentHealth;
        }
    }
}
