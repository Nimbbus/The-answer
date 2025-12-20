using UnityEngine;
using TMPro;
using System.Collections;

public class RegretDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Setup")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    [TextArea(2, 5)]
    public string[] dialogueLines;
    public float typingSpeed = 0.03f;

    private int currentLineIndex = 0;
    private bool dialogueActive = false;

    // ✅ New flag to ensure dialogue only plays once
    private bool dialoguePlayed = false;

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dialogueActive && !dialoguePlayed)
        {
            StartDialogue();
        }
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextLine();
        }
    }

    void StartDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        dialogueActive = true;
        dialoguePlayed = true; // ✅ Mark as played so it won’t trigger again
        currentLineIndex = 0;
        DisplayLine(dialogueLines[currentLineIndex]);

        Time.timeScale = 0f;
    }

    void DisplayNextLine()
    {
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
        StopAllCoroutines();
        StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        Time.timeScale = 1f;
    }
}
