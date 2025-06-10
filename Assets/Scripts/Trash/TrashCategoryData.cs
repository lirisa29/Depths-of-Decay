using UnityEngine;

public enum TrashCategory
{
    Light,
    Medium,
    Heavy
}

[CreateAssetMenu(fileName = "NewTrashCategoryData", menuName = "Trash/Trash Category Data")]
public class TrashCategoryData : ScriptableObject
{
    public TrashCategory categoryType;
    public float baseWeight;
}
