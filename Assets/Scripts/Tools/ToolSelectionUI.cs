using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ToolSelectionUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Transform selectionItemsContainer;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private ToolStoreDatabase toolDatabase;
    [SerializeField] private GameObject storeButton;
    [SerializeField] private GameObject maxItems;

    [Header("Layout Settings")]
    [SerializeField] private float itemSpacing = 10f;
    private float itemHeight;

    [Header("Store Events")]
    [SerializeField] private GameObject selectionUI;
    [SerializeField] private GameObject storeUI;
    
    [Space (20)]
    [Header("Scroll View")]
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject topScrollFade;
    [SerializeField] GameObject bottomScrollFade;

    private List<int> selectedToolIndices = new List<int>();
    private int allowedSlots;
    
    private LevelStartManager levelStartManager;
    
    [SerializeField] private TextMeshProUGUI allowedSlotsText;

    private void Start()
    {
        levelStartManager = FindFirstObjectByType<LevelStartManager>();
        
        allowedSlots = GameDataManager.GetAllowedToolSlots();
        allowedSlotsText.text = $"Limit: {allowedSlots}";
        
        selectedToolIndices.Clear();
        
        AddStoreEvents();
        GenerateSelectionUI();
    }

    void GenerateSelectionUI()
    {
        // Clear previous items
        itemHeight = itemPrefab.GetComponent<RectTransform>().sizeDelta.y;
        foreach (Transform child in selectionItemsContainer)
        {
            Destroy(child.gameObject);
        }
        
        selectedToolIndices.Clear();

        int visibleItemCount = 0;
        int defaultToolIndex = 0;

        for (int i = 0; i < toolDatabase.ToolsCount; i++)
        {
            Tool tool = toolDatabase.GetTool(i);
            
            if (!tool.isPurchased)
                continue;

            ToolItemUI uiItem = Instantiate(itemPrefab, selectionItemsContainer).GetComponent<ToolItemUI>();

            uiItem.SetItemPosition(Vector2.down * visibleItemCount * (itemHeight + itemSpacing));
            uiItem.gameObject.name = "Item" + i + "-" + tool.name;

            uiItem.SetToolPurchased();
            uiItem.SetToolName(tool.name);
            uiItem.SetToolImage(tool.image);
            uiItem.SetToolPros(tool.pros);
            uiItem.SetToolCons(tool.cons);

            // Handle selection logic
            int toolIndex = i;
            uiItem.OnItemSelect(toolIndex, OnItemSelected);
            
            if (i == defaultToolIndex)
            {
                selectedToolIndices.Add(defaultToolIndex);
                uiItem.SelectItem();
            }

            visibleItemCount++;
        }

        // Resize container height
        float margin = 20f;
        float totalHeight = visibleItemCount * (itemHeight + itemSpacing) - itemSpacing + margin * 2;

        selectionItemsContainer.GetComponent<RectTransform>().sizeDelta =
            new Vector2(selectionItemsContainer.GetComponent<RectTransform>().sizeDelta.x, Mathf.Max(totalHeight, 0f));
    }

    void OnItemSelected(int index)
    {
        ToolItemUI uiItem = GetItemUIByIndex(index);
        
        if (index == 0)
        {
            return;
        }

        if (selectedToolIndices.Contains(index))
        {
            selectedToolIndices.Remove(index);
            uiItem.DeselectItem();
        }
        else
        {
            if (selectedToolIndices.Count >= allowedSlots)
            {
                storeButton.SetActive(false);
                maxItems.SetActive(true);
                return;
            }

            storeButton.SetActive(true);
            maxItems.SetActive(false);
            
            selectedToolIndices.Add(index);
            uiItem.SelectItem();
        }
    }
    
    public void ConfirmToolSelection()
    {
        // Always include default tool (assume index 0 is default)
        if (!selectedToolIndices.Contains(0))
            selectedToolIndices.Insert(0, 0);

        GameDataManager.SetSelectedTools(selectedToolIndices); // You’ll need to implement this
        Debug.Log("Tool selection confirmed: " + string.Join(", ", selectedToolIndices));
        
        levelStartManager.OnToolSelectionConfirmed();
    }
    
    ToolItemUI GetItemUIByIndex (int index)
    {
        foreach (Transform child in selectionItemsContainer)
        {
            ToolItemUI itemUI = child.GetComponent<ToolItemUI>();
            if (child.name.StartsWith("Item" + index.ToString()))
                return itemUI;
        }

        return null;
    }
    
    void OnStoreListScroll (Vector2 value)
    {
        float scrollY = value.y;
        //Top fade 
        if (scrollY < 1f)
            topScrollFade.SetActive (true);
        else
            topScrollFade.SetActive (false);

        //Bottom fade 
        if (scrollY > 0f)
            bottomScrollFade.SetActive (true);
        else
            bottomScrollFade.SetActive (false);
    }

    public void OpenStoreUI()
    {
        selectionUI.SetActive (false);
        storeUI.SetActive (true);
    }

    public void CloseStoreUI()
    {
        GenerateSelectionUI();
        storeUI.SetActive (false);
        selectionUI.SetActive (true);
    }
    
    void AddStoreEvents()
    {
        scrollRect.onValueChanged.RemoveAllListeners ();
        //scrollRect.onValueChanged.AddListener (OnStoreListScroll);
    }
}
