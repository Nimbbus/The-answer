using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;
    public int currentLine = 0;

    private bool isDialogueActive = false;

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            NextLine();
        }
    }

    public void StartDialogue(string[] lines)
    {
        if (dialoguePanel == null || dialogueText == null || lines == null || lines.Length == 0)
        {
            Debug.LogWarning("DialogueManager setup is incomplete.");
            return;
        }

        isDialogueActive = true;
        dialoguePanel.SetActive(true);

        dialogueLines = lines;
        currentLine = 0;
        ShowLine();

        // ✅ Pause only if not in Old Man scene
        if (SceneManager.GetActiveScene().name != "OldManMeeting")
        {
            Time.timeScale = 0f;
        }

        // ✅ Cursor always hidden during dialogue
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ShowLine()
    {
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
    }

    public void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);

        if (SceneManager.GetActiveScene().name != "OldManMeeting")
        {
            Time.timeScale = 1f;
        }

        // ✅ Cursor stays hidden after dialogue ends
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
