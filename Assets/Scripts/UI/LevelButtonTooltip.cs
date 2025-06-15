using UnityEngine;
using UnityEngine.EventSystems;

public class LevelButtonTooltip : MonoBehaviour
{
    public string levelName;

    public void OnPointerEnter(PointerEventData eventData)
    {
        FindObjectOfType<LevelTooltip>()?.ShowTooltip(levelName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        FindObjectOfType<LevelTooltip>()?.HideTooltip();
    }
}
