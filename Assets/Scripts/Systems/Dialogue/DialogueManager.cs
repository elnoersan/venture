using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the display and progression of dialogue in the game.
/// This class handles showing dialogue boxes, animating text, and advancing through dialogue lines.
/// </summary>
public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject dialogueBox; // The UI element representing the dialogue box.
    [SerializeField] private GameObject nextLineIndicator; // The UI element indicating that the player can advance to the next line.
    [SerializeField] private TextMeshProUGUI dialogueText; // The TextMeshProUGUI component used to display dialogue text.
    [SerializeField] private int lettersPrSecond = 50; // The speed at which text is animated (letters per second).

    private int currentLine = 0; // The index of the current line being displayed.
    private Dialogue currentDialogue; // The current dialogue being shown.
    private bool isPrintingText; // Whether text is currently being animated.

    /// <summary>
    /// Initializes the DialogueManager by hiding the dialogue box at the start.
    /// </summary>
    private void Start()
    {
        dialogueBox.SetActive(false);
    }

    /// <summary>
    /// Shows a dialogue by activating the dialogue box and starting the animation of the first line.
    /// </summary>
    /// <param name="dialogue">The dialogue to be shown.</param>
    public void ShowDialog(Dialogue dialogue)
    {
        FindObjectOfType<GameEvents>().OnShowDialogInvoke();

        dialogueBox.SetActive(true);
        currentDialogue = dialogue;
        StartCoroutine(AnimateDialogue(dialogue.Lines[0]));
    }

    /// <summary>
    /// Animates the display of a dialogue line one character at a time.
    /// </summary>
    /// <param name="line">The line of text to animate.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    public IEnumerator AnimateDialogue(string line)
    {
        isPrintingText = true;
        nextLineIndicator.SetActive(false);

        dialogueText.text = "";
        foreach (var c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(1f / lettersPrSecond);
        }

        nextLineIndicator.SetActive(true);
        isPrintingText = false;
    }

    /// <summary>
    /// Advances to the next line of dialogue or closes the dialogue box if all lines have been shown.
    /// </summary>
    public void NextLine()
    {
        if (isPrintingText) return; // Alternatively, print all text at once to make it skippable.

        currentLine++;
        if (currentLine < currentDialogue.Lines.Count)
        {
            StartCoroutine(AnimateDialogue(currentDialogue.Lines[currentLine]));
        }
        else
        {
            currentLine = 0;
            dialogueBox.SetActive(false);
            FindObjectOfType<GameEvents>().OnCloseDialogInvoke();
        }
    }
}