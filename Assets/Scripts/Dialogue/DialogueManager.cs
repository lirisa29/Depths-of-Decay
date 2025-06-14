using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // The dialogue sequence containing all lines
    public DialogueSequence dialogueSequence;
    private IQueue<DialogueLine> dialogueQueue;
    
    // UI elements for displaying dialogue
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private Image speakerPortrait;
    
    // Buttons for navigating dialogue
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject raceButton;
    
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        dialogueQueue = new QueueArray<DialogueLine>();
        EnqueueDialogue();
        DequeueDialogue();
    }

    // Loads dialogue into the queue
    private void EnqueueDialogue()
    {
        if (dialogueSequence != null && dialogueSequence.dialogueLines != null)
        {
            foreach (var dialogueLine in dialogueSequence.dialogueLines)
            {
                dialogueQueue.Enqueue(dialogueLine);
            }
        }
    }

    // Displays the next dialogue line
    public void DequeueDialogue()
    {
        if (isTyping) // If typing is still in progress, stop it and start the next dialogue
        {
            StopCoroutine(typingCoroutine);
            isTyping = false;
        }
        
        if (!dialogueQueue.IsEmpty()) // Check if there are still lines to display
        {
            DialogueLine dialogueLine = dialogueQueue.Dequeue();
            DisplayDialogue(dialogueLine);
        }
        else
        {
            return; // Exit if no dialogue is left
        }

        // If there is only one dialogue left, swap next button for race button
        if (dialogueQueue.GetSize() < 1)
        {
            nextButton.SetActive(false);
            raceButton.SetActive(true);
        }
        
    }

    // Displays dialogue line with speaker info 
    private void DisplayDialogue(DialogueLine dialogueLine)
    {
        speakerNameText.text = dialogueLine.speaker.characterName; // Set speaker name
        speakerPortrait.sprite = dialogueLine.speaker.characterPortrait; // Set speaker portrait
        
        // Start typewriter effect if not already typing
        if (!isTyping)
        {
            typingCoroutine = StartCoroutine(TypeWriterEffect(dialogueLine.dialogueText));
        }
    }
    
    // Coroutine to display each character of the dialogue one by one
    private IEnumerator TypeWriterEffect(string dialogueTextToDisplay)
    {
        isTyping = true;
        dialogueText.text = ""; // Clear the text initially

        foreach (char letter in dialogueTextToDisplay)
        {
            dialogueText.text += letter; // Add one character at a time
            yield return new WaitForSeconds(0.05f); // Adjust typing speed here (lower is faster)
        }

        isTyping = false;
    }
}
