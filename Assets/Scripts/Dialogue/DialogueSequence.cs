using UnityEngine;

[CreateAssetMenu(fileName = "newDialogue", menuName = "Dialogue/DialogueSequence")]
public class DialogueSequence : ScriptableObject
{
    public DialogueLine[] dialogueLines;
}
