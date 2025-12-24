using UnityEngine;
using UnityEngine.SceneManagement;

// Portal that loads a scene when the player enters.
public class PortalToSecondBoss : MonoBehaviour
{
    [Tooltip("Name of the scene to load when player enters.")]
    // Scene name (set in Inspector)
    public string nextScene;

    // Called when a Collider enters this trigger
    private void OnTriggerEnter(Collider other)
    {
        // Only respond to the player
        if (other.CompareTag("Player"))
        {
            // Load the next scene
            SceneManager.LoadScene(nextScene);
        }
    }
}
