using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonOutlineEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image outlineImage;

    [Range(0f, 1f)] public float visibleAlpha = 1f; // Fully visible
    [Range(0f, 1f)] public float hiddenAlpha = 0f;  // Fully transparent
    
    private AudioManager audioManager;

    void Start()
    {
        SetAlpha(hiddenAlpha);
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetAlpha(visibleAlpha);
        audioManager.PlaySound("SFX_ButtonHover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetAlpha(hiddenAlpha);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetAlpha(visibleAlpha);
        audioManager.PlaySound("SFX_ButtonClick");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetAlpha(visibleAlpha);
    }
    
    public void ResetOutline()
    {
        SetAlpha(hiddenAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (outlineImage == null) return;
        
        Color c = outlineImage.color;
        c.a = alpha;
        outlineImage.color = c;
    }
}
