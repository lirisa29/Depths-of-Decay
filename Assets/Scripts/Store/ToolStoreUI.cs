using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToolStoreUI : MonoBehaviour
{
    [Header("Layout Settings")] 
    [SerializeField] private float itemSpacing = 10f;
    private float itemHeight;
    
    [Header("UI Elements")] 
    [SerializeField] private Transform storeItemsContainer;
    [SerializeField] private GameObject itemPrefab;
    [Space(20)] 
    [SerializeField] private ToolStoreDatabase toolDatabase;
    [SerializeField] private GameObject noMorePoints;
    
    [Space (20)]
    [Header("Scroll View")]
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject topScrollFade;
    [SerializeField] GameObject bottomScrollFade;

    [Space (20)]
    [Header ("Error messages")]
    [SerializeField] TextMeshProUGUI noEnoughPointsText;
    
    private AudioManager audioManager;
    
    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        AddStoreEvents();
        GenerateStoreItemUI();
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
            
            float margin = 20f; // Customize this as needed
            float totalHeight = visibleItemCount * (itemHeight + itemSpacing) - itemSpacing + margin * 2;

            storeItemsContainer.GetComponent<RectTransform>().sizeDelta =
                new Vector2(storeItemsContainer.GetComponent<RectTransform>().sizeDelta.x, Mathf.Max(totalHeight, 0f));
        }
    }
    
    ToolItemUI GetItemUI (int index)
    {
        return storeItemsContainer.GetChild (index).GetComponent <ToolItemUI> ();
    }

    void OnItemPurchase(int index, ToolItemUI uiItem)
    {
        Tool tool = toolDatabase.GetTool (index);

        if (GameDataManager.CanSpendPoints (tool.price)) {
            audioManager.PlaySound("SFX_ToolPurchased");
            
            //Proceed with the purchase operation
            GameDataManager.SpendPoints (tool.price);

            //Update Points UI text
            GameSharedUI.Instance.UpdatePointsUIText ();

            //Update DB's Data
            toolDatabase.PurchaseTool(index);

            uiItem.SetToolPurchased();

            //Add purchased item to Shop Data
            GameDataManager.AddPurchasedTool(index);
            
            GenerateStoreItemUI();

        } 
        else 
        {
            //No enough coins..
            StartCoroutine(AnimateNoMoreCoinsText());
        }
    }
    
    private IEnumerator AnimateNoMoreCoinsText()
    {
        noMorePoints.SetActive(true);
        yield return new WaitForSeconds(2f);
        noMorePoints.SetActive(false);
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
        scrollRect.onValueChanged.RemoveAllListeners();
        scrollRect.onValueChanged.AddListener(OnStoreListScroll);
    }
}
