using UnityEngine;

public class TrashItem : MonoBehaviour
{
    public TrashItemData itemData;

    public float GetWeight() => itemData.Weight;
    public string GetTrashName() => itemData.trashName;
    public TrashCategory GetCategory() => itemData.categoryData.categoryType;
}
