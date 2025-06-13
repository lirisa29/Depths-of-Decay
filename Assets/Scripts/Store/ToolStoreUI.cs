using UnityEngine;
using UnityEngine.UI;

public class ToolStoreUI : MonoBehaviour
{
    [Header("Layout Settings")] 
    [SerializeField] private float itemSpacing = 10f;
    private float itemHeight;
    
    [Header("UI Elements")] 
    [SerializeField] private Transform storeMenu;
    [SerializeField] private Transform storeItemsContainer;
    [SerializeField] private GameObject itemPrefab;
    [Space(20)] 
    [SerializeField] private ToolStoreDatabase toolDatabase;
    
    [Header("Store Events")] 
    [SerializeField] private GameObject storeUI;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] Button openStoreButton;
    [SerializeField] Button closeStoreButton;

    void Start()
    {
        AddStoreEvents();
        GenerateStoreItemUI();
    }

    void GenerateStoreItemUI()
    {
        itemHeight = storeItemsContainer.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        Destroy(storeItemsContainer.GetChild(0).gameObject);
        storeItemsContainer.DetachChildren();

        for (int i = 0; i < toolDatabase.ToolsCount; i++)
        {
            Tool tool = toolDatabase.GetTool(i);
            ToolItemUI uiItem = Instantiate(itemPrefab, storeItemsContainer).GetComponent<ToolItemUI>();
            
            uiItem.SetItemPosition(Vector2.down * i * (itemHeight + itemSpacing));
            
            uiItem.gameObject.name = "Item" + i + "-" + tool.name;
            
            uiItem.SetToolName(tool.name);
            uiItem.SetToolImage(tool.image);
            uiItem.SetToolPros(tool.pros);
            uiItem.SetToolCons(tool.cons);
            uiItem.SetToolCost(tool.price);

            if (tool.isPurchased)
            {
                uiItem.SetToolPurchased();
                uiItem.OnItemSelect(i, OnItemSelected);
            }
            else
            {
                uiItem.SetToolCost(tool.price);
                uiItem.OnItemPurchased(i, OnItemPurchase);
            }
            
            storeItemsContainer.GetComponent<RectTransform>().sizeDelta =
                Vector2.up * ((itemHeight + itemSpacing) * toolDatabase.ToolsCount + 25);
        }
    }

    void OnItemSelected(int index)
    {
        Debug.Log("Select" + index);
    }

    void OnItemPurchase(int index)
    {
        Debug.Log("Purchase" + index);
    }
    
    void AddStoreEvents()
    {
        openStoreButton.onClick.RemoveAllListeners();
        openStoreButton.onClick.AddListener(OpenStore);
        
        closeStoreButton.onClick.RemoveAllListeners();
        closeStoreButton.onClick.AddListener(CloseStore);
    }

    void OpenStore()
    {
        storeUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    void CloseStore()
    {
        storeUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }
}
