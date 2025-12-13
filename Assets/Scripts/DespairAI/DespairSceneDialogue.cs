using UnityEngine;

public class DespairSceneDialogue : MonoBehaviour
{
    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] lines;

    [Header("References")]
    public DialogueManager dialogueManager; // assign in Inspector

    private bool hasTriggered = false;

    void Awake()
    {
        // Fallback: auto-find if not assigned
        if (dialogueManager == null)
            dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (dialogueManager == null) return;

        dialogueManager.StartDialogue(lines);
        hasTriggered = true;
    }
}
