using TMPro;
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
    
    [Space (20)]
    [Header("Scroll View")]
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject topScrollFade;
    [SerializeField] GameObject bottomScrollFade;

    [Space (20)]
    [Header ("Error messages")]
    [SerializeField] TextMeshProUGUI noEnoughPointsText;

    int newSelectedItemIndex = 0;
    int previousSelectedItemIndex = 0;

    void Start()
    {
        AddStoreEvents();
        GenerateStoreItemUI();
        SetSelectedTool();
        SelectItemUI(GameDataManager.GetSelectedToolIndex());
    }
    
    void SetSelectedTool ()
    {
        //Get saved index
        int index = GameDataManager.GetSelectedToolIndex ();

        //Set selected tool
        GameDataManager.SetSelectedTool (toolDatabase.GetTool(index), index);
    }

    void GenerateStoreItemUI()
    {
        itemHeight = storeItemsContainer.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        Destroy(storeItemsContainer.GetChild(0).gameObject);
        storeItemsContainer.DetachChildren();
        
        int visibleItemCount = 0;

        for (int i = 0; i < toolDatabase.ToolsCount; i++)
        {
            Tool tool = toolDatabase.GetTool(i);
            
            if (tool.isPurchased)
                continue;
            
            ToolItemUI uiItem = Instantiate(itemPrefab, storeItemsContainer).GetComponent<ToolItemUI>();
            
            uiItem.SetItemPosition(Vector2.down * visibleItemCount * (itemHeight + itemSpacing));
            
            uiItem.gameObject.name = "Item" + i + "-" + tool.name;
            
            uiItem.SetToolName(tool.name);
            uiItem.SetToolImage(tool.image);
            uiItem.SetToolPros(tool.pros);
            uiItem.SetToolCons(tool.cons);
            uiItem.SetToolCost(tool.price);
            uiItem.OnItemPurchased(i, OnItemPurchase);
            
            visibleItemCount++;
            
            float margin = 10f; // Customize this as needed
            float totalHeight = visibleItemCount * (itemHeight + itemSpacing) - itemSpacing + margin * 2;

            storeItemsContainer.GetComponent<RectTransform>().sizeDelta =
                new Vector2(storeItemsContainer.GetComponent<RectTransform>().sizeDelta.x, Mathf.Max(totalHeight, 0f));
        }
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
        return storeItemsContainer.GetChild (index).GetComponent <ToolItemUI> ();
    }

    void OnItemPurchase(int index)
    {
        Tool tool = toolDatabase.GetTool (index);
        ToolItemUI uiItem = GetItemUI (index);

        if (GameDataManager.CanSpendPoints (tool.price)) {
            //Proceed with the purchase operation
            GameDataManager.SpendPoints (tool.price);

            //Update Points UI text
            GameSharedUI.Instance.UpdatePointsUIText ();

            //Update DB's Data
            toolDatabase.PurchaseTool(index);

            uiItem.SetToolPurchased();
            uiItem.OnItemSelect (index, OnItemSelected);

            //Add purchased item to Shop Data
            GameDataManager.AddPurchasedTool(index);

        } else {
            //No enough coins..
            AnimateNoMoreCoinsText ();
        }
    }
    
    void AnimateNoMoreCoinsText ()
    {
       /* // Complete animations (if it's running)
        noEnoughPointsText.transform.DOComplete ();
        noEnoughPointsText.DOComplete ();

        noEnoughPointsText.transform.DOShakePosition (3f, new Vector3 (5f, 0f, 0f), 10, 0);
        noEnoughPointsText.DOFade (1f, 3f).From (0f).OnComplete (() => {
            noEnoughPointsText.DOFade (0f, 1f);
        }); */
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
        openStoreButton.onClick.RemoveAllListeners();
        openStoreButton.onClick.AddListener(OpenStore);
        
        closeStoreButton.onClick.RemoveAllListeners();
        closeStoreButton.onClick.AddListener(CloseStore);
        
        scrollRect.onValueChanged.RemoveAllListeners ();
        scrollRect.onValueChanged.AddListener (OnStoreListScroll);
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
