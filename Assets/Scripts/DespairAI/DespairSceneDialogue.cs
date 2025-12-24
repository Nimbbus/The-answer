using UnityEngine;

public class DespairSceneDialogue : MonoBehaviour
{
    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] lines; // dialogue lines to show

    [Header("References")]
    public DialogueManager dialogueManager; // dialogue system reference

    private bool hasTriggered = false; // ensure dialogue only triggers once

    void Awake()
    {
        // find DialogueManager if not set
        if (dialogueManager == null)
            dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        // only trigger once, only for player, and require manager
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (dialogueManager == null) return;

        // start the dialogue and mark triggered
        dialogueManager.StartDialogue(lines);
        hasTriggered = true;
    }
}
