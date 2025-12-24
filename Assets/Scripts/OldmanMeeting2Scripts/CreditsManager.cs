using UnityEngine;
using TMPro;
using System.Collections;

public class CreditsManager : MonoBehaviour
{
    [Header("Credits Settings")]
    public GameObject creditsPanel;        // credits UI panel
    public TMP_Text creditsText;           // text element to display lines

    [TextArea(2, 5)]
    public string[] creditLines;           // lines to type (set in Inspector)

    public float typingSpeed = 30f;        // characters per second
    public float lineDuration = 5f;        // seconds to keep line visible
    public float endDelay = 2f;            // delay before quitting

    void Start()
    {
        // hide credits UI at start
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    public void StartCredits()
    {
        // validate and begin credits
        if (creditsPanel == null || creditsText == null || creditLines.Length == 0) return;

        creditsPanel.SetActive(true);
        Time.timeScale = 0f; // pause gameplay during credits

        StartCoroutine(PlayCredits());
    }

    IEnumerator PlayCredits()
    {
        // iterate each credit line
        foreach (string line in creditLines)
        {
            // clear text then type characters one-by-one
            creditsText.text = "";
            foreach (char c in line)
            {
                creditsText.text += c; // add next char
                yield return new WaitForSecondsRealtime(1f / typingSpeed); // use realtime so pause doesn't affect timing
            }

            // hold the fully typed line for a moment
            yield return new WaitForSecondsRealtime(lineDuration);
        }

        // finished all lines — wait then quit
        yield return new WaitForSecondsRealtime(endDelay);
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // stop play mode in editor
#endif
    }
}
