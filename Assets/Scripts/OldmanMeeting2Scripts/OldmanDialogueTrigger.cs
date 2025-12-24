using UnityEngine;
using TMPro;
using System.Collections;

public class OldmanDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [Tooltip("Assign the dialogue panel (Canvas) here.")]
    public GameObject dialoguePanel; // dialogue UI panel

    [Tooltip("Assign the TextMeshPro UI element that displays dialogue.")]
    public TMP_Text dialogueText; // text element for lines

    [Tooltip("Add dialogue lines here in the Inspector.")]
    [TextArea(2, 5)]
    public string[] dialogueLines; // lines to show

    [Tooltip("Key used to progress dialogue.")]
    public KeyCode advanceKey = KeyCode.E; // key to advance dialogue

    [Tooltip("Speed of typewriter effect (characters per second).")]
    public float typingSpeed = 30f; // typing speed (chars/sec)

    [Header("Credits Manager")]
    [Tooltip("Assign the CreditsManager script here.")]
    public CreditsManager creditsManager; // optional credits starter

    private int currentLineIndex = 0; // current dialogue index
    private bool playerInRange = false; // is player inside trigger
    private bool dialogueActive = false; // is dialogue open
    private bool isTyping = false; // is typewriter coroutine running

    void Start()
    {
        // hide dialogue UI at start
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void Update()
    {
        // handle advance/skip input while dialogue is active and player nearby
        if (dialogueActive && playerInRange)
        {
            if (Input.GetKeyDown(advanceKey))
            {
                if (isTyping)
                {
                    // stop typing and show full current line
                    StopAllCoroutines();
                    dialogueText.text = dialogueLines[currentLineIndex];
                    isTyping = false;
                }
                else
                {
                    // move to next line or end
                    ShowNextLine();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // start dialogue when player enters
        if (other.CompareTag("Player") && !dialogueActive)
        {
            playerInRange = true;
            StartDialogue();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // end dialogue when player leaves
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            EndDialogue();
        }
    }

    void StartDialogue()
    {
        // validate and open dialogue UI, then type first line
        if (dialoguePanel == null || dialogueText == null || dialogueLines.Length == 0) return;

        dialoguePanel.SetActive(true);
        dialogueActive = true;
        currentLineIndex = 0;

        StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
    }

    void ShowNextLine()
    {
        // advance index and either type next line or finish
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
        {
            StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
        }
        else
        {
            EndDialogue();

            // start credits if assigned
            if (creditsManager != null)
            {
                creditsManager.StartCredits();
            }
        }
    }

    IEnumerator TypeLine(string line)
    {
        // type characters one-by-one (timing affected by Time.timeScale)
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(1f / typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        // close UI and reset state
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        dialogueActive = false;
        currentLineIndex = 0;
    }
}