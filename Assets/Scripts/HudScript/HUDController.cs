using UnityEngine;
using UnityEngine.UI;

// Updates on-screen health bars
public class HUDController : MonoBehaviour
{
    [Header("References")]
    public Slider playerHealthBar; // UI slider for player health
    public Slider bossHealthBar;   // UI slider for boss health

    private PlayerHealth playerHealth; // cached player health component
    private BossHealth bossHealth;     // cached boss health component

    void Start()
    {
        // find health components in the scene
        playerHealth = FindObjectOfType<PlayerHealth>();
        bossHealth = FindObjectOfType<BossHealth>();

        // initialize player slider
        if (playerHealthBar != null)
        {
            playerHealthBar.maxValue = playerHealth.maxHealth; // set max
            playerHealthBar.value = playerHealth.CurrentHealth; // set current
        }

        // initialize boss slider
        if (bossHealthBar != null)
        {
            bossHealthBar.maxValue = bossHealth.maxHealth; // set max
            bossHealthBar.value = bossHealth.CurrentHealth; // set current
        }
    }

    void Update()
    {
        // update player slider value each frame
        if (playerHealth != null && playerHealthBar != null)
        {
            playerHealthBar.value = playerHealth.CurrentHealth;
        }

        // update boss slider value each frame
        if (bossHealth != null && bossHealthBar != null)
        {
            bossHealthBar.value = bossHealth.CurrentHealth;
        }
    }
}
