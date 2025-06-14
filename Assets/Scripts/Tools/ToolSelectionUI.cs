using UnityEngine;
using UnityEngine.UI;

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

    private int newSelectedItemIndex = 0;
    private int previousSelectedItemIndex = 0;

    private void Start()
    {
        SetSelectedTool();
        AddStoreEvents();
        GenerateSelectionUI();
    }
    
    void SetSelectedTool ()
    {
        //Get saved index
        int index = GameDataManager.GetSelectedToolIndex ();

        //Set selected tool
        GameDataManager.SetSelectedTool (toolDatabase.GetTool(index), index);
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
            uiItem.OnItemSelect(i, OnItemSelected);

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
        // Select item in the UI
        SelectItemUI (index);

        //Save Data
        GameDataManager.SetSelectedTool (toolDatabase.GetTool(index), index);
    }
    
    void SelectItemUI (int itemIndex)
    {
        previousSelectedItemIndex = newSelectedItemIndex;
        newSelectedItemIndex = itemIndex;

        ToolItemUI prevUiItem = GetItemUI (previousSelectedItemIndex);
        ToolItemUI newUiItem = GetItemUI (newSelectedItemIndex);

        prevUiItem.DeselectItem ();
        newUiItem.SelectItem ();

    }
    
    ToolItemUI GetItemUI (int index)
    {
        return selectionItemsContainer.GetChild (index).GetComponent <ToolItemUI> ();
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
