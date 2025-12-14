using UnityEngine;
using UnityEngine.UI;

public class DespairSceneHUD : MonoBehaviour
{
    [Header("UI")]
    public Slider travellerHealthBar;
    public Slider bossHealthBar;

    [Header("References")]
    public DespairSceneHealth travellerHealth;
    public DespairSceneHealth bossHealth;

    void Start()
    {
        if (travellerHealthBar != null && travellerHealth != null)
        {
            travellerHealthBar.maxValue = travellerHealth.maxHealth;
            travellerHealthBar.value = travellerHealth.currentHealth;
        }

        if (bossHealthBar != null && bossHealth != null)
        {
            bossHealthBar.maxValue = bossHealth.maxHealth;
            bossHealthBar.value = bossHealth.currentHealth;
        }
    }

    void Update()
    {
        if (travellerHealthBar != null && travellerHealth != null)
        {
            travellerHealthBar.value = travellerHealth.currentHealth;
        }

        if (bossHealthBar != null && bossHealth != null)
        {
            bossHealthBar.value = bossHealth.currentHealth;
        }
    }
}
