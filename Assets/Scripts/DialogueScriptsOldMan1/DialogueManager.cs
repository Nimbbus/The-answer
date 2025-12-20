using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue Data")]
    public string[] dialogueLines;
    public int currentLine = 0;

    [Header("Typing Effect")]
    public float typingSpeed = 0.03f; // ✅ adjustable typing speed
    private Coroutine typingCoroutine;

    private bool isDialogueActive = false;

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            // ✅ If typing is still running, skip to full line instantly
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
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeLine(dialogueLines[currentLine]));
        }
    }

    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        typingCoroutine = null; // ✅ reset when finished
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
