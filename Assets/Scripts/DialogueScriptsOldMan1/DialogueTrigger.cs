using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager; // reference to the dialogue manager
    [TextArea(3, 10)]
    public string[] dialogueLines; // lines to display

    void OnTriggerEnter(Collider other)
    {
        // start dialogue when player enters
        if (other.CompareTag("Player"))
        {
            dialogueManager.StartDialogue(dialogueLines);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // end dialogue when player leaves
        if (other.CompareTag("Player"))
        {
            dialogueManager.EndDialogue();
        }
    }
}
