using UnityEngine;
using TMPro;
using System.Collections;

public class BossDialogueTrigger : MonoBehaviour
{
    public GameObject dialoguePanel;
    public string[] dialogueLines;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.03f; // ✅ adjustable typing speed

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
        hasPlayed = true; // ✅ Prevent retrigger
        dialoguePanel.SetActive(true);
        Time.timeScale = 0f;

        currentLine = 0;
        DisplayLine(dialogueLines[currentLine]);
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            DisplayLine(dialogueLines[currentLine]);
        }
        else
        {
            EndDialogue();
        }
    }

    void DisplayLine(string line)
    {
        StopAllCoroutines(); // ✅ stop any ongoing typing
        StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
            // ✅ WaitForSecondsRealtime ensures typing works while Time.timeScale = 0
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
