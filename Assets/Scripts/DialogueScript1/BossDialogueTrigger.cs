using UnityEngine;

public class BossDialogueTrigger : MonoBehaviour
{
    public GameObject dialoguePanel;
    public string[] dialogueLines;
    public TMPro.TextMeshProUGUI dialogueText;

    private int currentLine = 0;
    private bool isDialogueActive = false;
    private bool hasPlayed = false; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDialogueActive && !hasPlayed)
        {
            StartDialogue();
        }
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            NextLine();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        hasPlayed = true; // Prevent retrigger
        dialoguePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        currentLine = 0;
        dialogueText.text = dialogueLines[currentLine];
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        
    }
}
