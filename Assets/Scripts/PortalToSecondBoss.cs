using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalToSecondBoss : MonoBehaviour
{
    [Tooltip("Name of the scene to load when player enters.")]
    public string nextScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered portal. Loading scene: " + nextScene);
            SceneManager.LoadScene(nextScene);
        }
    }
}
