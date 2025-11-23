using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public int currentLine;
    private string[] lines;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }
    public void StartDialogue(string[] dialogueLines)
    {
        lines = dialogueLines;
        currentLine = 0;
        dialoguePanel.SetActive(true);
        ShowLine();
    }

    public void ShowLine()
    {
        if (currentLine < lines.Length)
        {
            dialogueText.text = lines[currentLine];
        }
        else
        {
            EndDialogue();
        }
    }


    void Update()
    {
        
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            NextLine();
        }
    }

    public void NextLine()
    {
        currentLine++;
        ShowLine();
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
