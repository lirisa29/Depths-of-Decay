using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToolSelectionUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Transform selectionItemsContainer;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private ToolStoreDatabase toolDatabase;

    [Header("Layout Settings")]
    [SerializeField] private float itemSpacing = 10f;
    private float itemHeight;

    [Header("Store Events")]
    [SerializeField] private GameObject selectionUI;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private Button openSelectionButton;
    [SerializeField] private Button closeSelectionButton;
    
    [Space (20)]
    [Header("Scroll View")]
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject topScrollFade;
    [SerializeField] GameObject bottomScrollFade;

    private List<int> selectedToolIndices = new List<int>();
    private int allowedSlots;

    private void Start()
    {
        allowedSlots = GameDataManager.GetAllowedToolSlots();
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

        int visibleItemCount = 0;

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

        if (selectedToolIndices.Contains(index))
        {
            selectedToolIndices.Remove(index);
            uiItem.DeselectItem();
        }
        else
        {
            if (selectedToolIndices.Count >= allowedSlots)
            {
                Debug.LogWarning("Maximum tools selected");
                // Optional: flash warning in UI
                return;
            }

            selectedToolIndices.Add(index);
            uiItem.SelectItem();
        }
    }
    
    public void ConfirmToolSelection()
    {
        // Always include default tool (assume index 0 is default)
        if (!selectedToolIndices.Contains(0))
            selectedToolIndices.Insert(0, 0);

        List<Tool> selectedTools = new List<Tool>();
        foreach (int index in selectedToolIndices)
        {
            selectedTools.Add(toolDatabase.GetTool(index));
        }

        GameDataManager.SetSelectedTools(selectedTools, selectedToolIndices); // You’ll need to implement this
        Debug.Log("Tool selection confirmed: " + string.Join(", ", selectedToolIndices));
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
    
    void AddStoreEvents()
    {
        scrollRect.onValueChanged.RemoveAllListeners ();
        scrollRect.onValueChanged.AddListener (OnStoreListScroll);
    }
}
