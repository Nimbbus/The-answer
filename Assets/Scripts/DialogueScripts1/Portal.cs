using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene loading

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Rage"); // Load the Rage scene
        }
    }
}
