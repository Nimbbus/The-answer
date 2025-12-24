using UnityEngine;
using TMPro;
using System.Collections;

public class RegretDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Setup")]
    public GameObject dialoguePanel;               // dialogue UI panel
    public TextMeshProUGUI dialogueText;           // text element to show lines
    [TextArea(2, 5)]
    public string[] dialogueLines;                 // lines to display
    public float typingSpeed = 0.03f;              // delay between characters (seconds)

    private int currentLineIndex = 0;              // index of current line
    private bool dialogueActive = false;           // is dialogue open
    private bool dialoguePlayed = false;           // prevent retriggering

    void Start()
    {
        // hide dialogue at start
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // start dialogue when player enters and it hasn't played
        if (other.CompareTag("Player") && !dialogueActive && !dialoguePlayed)
        {
            StartDialogue();
        }
    }

    void Update()
    {
        // advance dialogue on E while active
        if (dialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextLine();
        }
    }

    void StartDialogue()
    {
        // show UI, pause game and start first line
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        dialogueActive = true;
        dialoguePlayed = true; // mark as played once
        currentLineIndex = 0;
        DisplayLine(dialogueLines[currentLineIndex]);

        Time.timeScale = 0f; // pause game
    }

    void DisplayNextLine()
    {
        // go to next line or finish
        currentLineIndex++;
        if (currentLineIndex < dialogueLines.Length)
        {
            DisplayLine(dialogueLines[currentLineIndex]);
        }
        else
        {
            EndDialogue();
        }
    }

    void DisplayLine(string line)
    {
        // restart typing coroutine for this line
        StopAllCoroutines();
        StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        // type text character-by-character using realtime
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
        dialogueActive = false;
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        Time.timeScale = 1f; // resume game
    }
}
