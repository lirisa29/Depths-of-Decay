using UnityEngine;
using TMPro;

public class LevelTooltip : MonoBehaviour
{
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;
    public Vector2 offset;

    void Update()
    {
        if (tooltipPanel.activeSelf)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tooltipPanel.transform.parent as RectTransform,
                Input.mousePosition,
                null,
                out pos
            );
            tooltipPanel.GetComponent<RectTransform>().anchoredPosition = pos + offset;
        }
    }

    public void ShowTooltip(string text)
    {
        tooltipText.text = text;
        tooltipPanel.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
