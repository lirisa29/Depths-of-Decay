using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public CharacterInfo speaker;
    [TextArea(3, 10)] public string dialogueText;
}
