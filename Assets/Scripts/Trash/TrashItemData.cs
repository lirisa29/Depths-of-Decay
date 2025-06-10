using UnityEngine;

[CreateAssetMenu(fileName = "TrashItemData", menuName = "Trash/Trash Item Data")]
public class TrashItemData : ScriptableObject
{
    public string trashName;
    public TrashCategoryData categoryData;
    
    // Derived properties - read-only access to values from the categoryData
    public float Weight => categoryData != null ? categoryData.baseWeight : 0f;
}
