using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;

public class ToolItemUI : MonoBehaviour
{
    [SerializeField] private Color itemNotSelectedColour;
    [SerializeField] private Color itemSelectedColour;
    
    [Space (20f)]
    [SerializeField] private Image toolImage;
    [SerializeField] private TextMeshProUGUI toolNameText;
    [SerializeField] private TextMeshProUGUI toolProsText;
    [SerializeField] private TextMeshProUGUI toolConsText;
    [SerializeField] private TextMeshProUGUI toolCostText;
    [SerializeField] Button toolPurchaseButton;
    
    [Space (20f)]
    [SerializeField] Button itemButton;
    [SerializeField] private Image itemImage;
    //[SerializeField] private Outline itemOutline;

    public void SetItemPosition(Vector2 pos)
    {
        GetComponent<RectTransform>().anchoredPosition += pos;
    }

    public void SetToolImage(Sprite image)
    {
        toolImage.sprite = image;
    }

    public void SetToolName(string toolName)
    {
        toolNameText.text = toolName;
    }

    public void SetToolPros(string pros)
    {
        toolProsText.text = pros;
    }
    
    public void SetToolCons(string cons)
    {
        toolConsText.text = cons;
    }

    public void SetToolCost(int cost)
    {
        toolCostText.text = cost.ToString();
    }

    public void SetToolPurchased()
    {
        toolPurchaseButton.gameObject.SetActive(false);
        itemButton.interactable = true;
        
        itemImage.color = itemNotSelectedColour;
    }

    public void OnItemPurchased(int itemIndex, UnityAction<int, ToolItemUI> action)
    {
        toolPurchaseButton.onClick.RemoveAllListeners();
        toolPurchaseButton.onClick.AddListener(() => action.Invoke(itemIndex, this));
    }

    public void OnItemSelect(int itemIndex, UnityAction<int> action)
    {
        itemButton.interactable = true;
        itemButton.onClick.RemoveAllListeners();
        itemButton.onClick.AddListener(() => action.Invoke(itemIndex));
    }

    public void SelectItem()
    {
        //itemOutline.enabled = true;
        itemImage.color = itemSelectedColour;
    }
    
    public void DeselectItem()
    {
        //itemOutline.enabled = false;
        itemImage.color = itemNotSelectedColour;
        itemButton.interactable = true;
    }
}
