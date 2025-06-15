using UnityEngine;
using UnityEngine.EventSystems;

public class LevelButtonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string levelName;
    public LevelTooltip tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip(levelName, GetComponent<RectTransform>());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
