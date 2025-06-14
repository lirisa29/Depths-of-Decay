using UnityEngine;

[CreateAssetMenu(fileName = "newCharacter", menuName = "Dialogue/CharacterInfo")]
public class CharacterInfo : ScriptableObject
{
    public string characterName;
    public Sprite characterPortrait;
}
