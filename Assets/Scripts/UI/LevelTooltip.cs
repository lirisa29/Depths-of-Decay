using UnityEngine;
using TMPro;

public class LevelTooltip : MonoBehaviour
{
    public GameObject tooltipPrefab;
    private GameObject currentTooltip;
    public Vector2 offset;
    public Transform tooltipParent;

    // Call this to show tooltip above the button
    public void ShowTooltip(string text, RectTransform targetButton)
    {
        if (currentTooltip != null)
            Destroy(currentTooltip);

        // Instantiate the tooltip under the same canvas
        currentTooltip = Instantiate(tooltipPrefab, tooltipParent);

        // Set the text
        TextMeshProUGUI tooltipText = currentTooltip.GetComponentInChildren<TextMeshProUGUI>();
        tooltipText.text = text;

        // Position it above the button
        RectTransform tooltipRect = currentTooltip.GetComponent<RectTransform>();

        // Get screen position of top center of the button
        Vector3[] corners = new Vector3[4];
        targetButton.GetWorldCorners(corners);
        Vector3 buttonTopCenter = (corners[1] + corners[2]) / 2f;

        // Convert world point to screen point
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, buttonTopCenter);

        // Convert to local position within parent canvas
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            tooltipParent as RectTransform,
            screenPos,
            null, // no camera needed for Screen Space - Overlay
            out localPos
        );

        tooltipRect.anchoredPosition = localPos + new Vector2(0, 20);
    }

    public void HideTooltip()
    {
        if (currentTooltip != null)
        {
            Destroy(currentTooltip);
        }
    }
}
