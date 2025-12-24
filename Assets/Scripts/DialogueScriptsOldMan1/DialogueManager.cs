using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;           // UI panel to show dialogue
    public TextMeshProUGUI dialogueText;       // text element for typing

    [Header("Dialogue Data")]
    public string[] dialogueLines;             // lines to display
    public int currentLine = 0;                // index of current line

    [Header("Typing Effect")]
    public float typingSpeed = 0.03f;          // delay between characters
    private Coroutine typingCoroutine;         // active typing coroutine

    private bool isDialogueActive = false;     // whether dialogue is open

    void Update()
    {
        // advance or skip typing when player presses E
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            // skip typing and show full line if typing
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogueLines[currentLine];
                typingCoroutine = null;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void StartDialogue(string[] lines)
    {
        // validate inputs
        if (dialoguePanel == null || dialogueText == null || lines == null || lines.Length == 0)
        {
            return;
        }

        isDialogueActive = true;
        dialoguePanel.SetActive(true);

        dialogueLines = lines;
        currentLine = 0;
        ShowLine();

        // pause game except for OldManMeeting scene
        if (SceneManager.GetActiveScene().name != "OldManMeeting")
        {
            Time.timeScale = 0f;
        }
    }

    private void ShowLine()
    {
        // start typing current line
        if (currentLine < dialogueLines.Length)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeLine(dialogueLines[currentLine]));
        }
    }

    private IEnumerator TypeLine(string line)
    {
        // type text char-by-char (uses realtime so it works while paused)
        dialogueText.text = "";
        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        typingCoroutine = null; // typing finished
    }

    public void NextLine()
    {
        // move to next line or end dialogue
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
        // close UI and resume game if needed
        isDialogueActive = false;
        dialoguePanel.SetActive(false);

        if (SceneManager.GetActiveScene().name != "OldManMeeting")
        {
            Time.timeScale = 1f;
        }
    }
}
