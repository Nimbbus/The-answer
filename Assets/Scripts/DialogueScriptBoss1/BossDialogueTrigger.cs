using UnityEngine;
using TMPro;
using System.Collections;

public class BossDialogueTrigger : MonoBehaviour
{
    public GameObject dialoguePanel;            // panel that shows dialogue UI
    public string[] dialogueLines;              // lines to display
    public TextMeshProUGUI dialogueText;        // UI text element
    public float typingSpeed = 0.03f;           // time between typed characters

    private int currentLine = 0;                // current line index
    private bool isDialogueActive = false;      // is dialogue open
    private bool hasPlayed = false;             // prevent retriggering

    void OnTriggerEnter(Collider other)
    {
        // start when player enters trigger
        if (other.CompareTag("Player") && !isDialogueActive && !hasPlayed)
        {
            StartDialogue();
        }
    }

    void Update()
    {
        // advance line on E while dialogue is active
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            NextLine();
        }
    }

    void StartDialogue()
    {
        // open UI, pause game, begin first line
        isDialogueActive = true;
        hasPlayed = true; // only once
        dialoguePanel.SetActive(true);
        Time.timeScale = 0f;

        currentLine = 0;
        DisplayLine(dialogueLines[currentLine]);
    }

    void NextLine()
    {
        // go to next line or end
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
        // stop previous typing and start new one
        StopAllCoroutines();
        StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        // type text character-by-character (works while timeScale = 0)
        dialogueText.text = "";
        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }

    void EndDialogue()
    {
        // close UI and resume game
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
