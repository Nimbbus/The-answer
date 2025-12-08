using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue Data")]
    public string[] dialogueLines;
    public int currentLine = 0;

    private bool isDialogueActive = false;

    void Update()
    {
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

        // ✅ Pause gameplay unless in Old Man scene
        if (SceneManager.GetActiveScene().name != "OldManMeeting")
        {
            Time.timeScale = 0f;
        }
    }

    private void ShowLine()
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
    }
}
