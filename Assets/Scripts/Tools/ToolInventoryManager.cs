using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ToolInventoryManager : MonoBehaviour
{
    private List<Tool> equippedTools;
    private int currentToolIndex = 0;
    private bool inventoryOpen = false;
    
    [SerializeField] private PlayerController playerController;
    [SerializeField] private OxygenManager oxygenManager;
    [SerializeField] private TrashCollection trashCollection;

    private float baseOxygenRate;
    private int baseCarryLimit;
    
    [SerializeField] private GameObject inventoryPanel; // UI parent panel
    [SerializeField] private List<Image> toolIcons;     // List of UI Image components to show tool icons
    [SerializeField] private List<Image> slotBackgrounds; // Backgrounds to change color

    [SerializeField] private Color defaultSlotColor;
    [SerializeField] private Color equippedSlotColor;
    
    [SerializeField] private ToolStoreDatabase toolDatabase;
    
    void Start()
    {
        GameDataManager.toolDatabase = toolDatabase;
        
        playerController = FindFirstObjectByType<PlayerController>();
        oxygenManager = FindFirstObjectByType<OxygenManager>();
        trashCollection = FindFirstObjectByType<TrashCollection>();

        RefreshEquippedTools();
        
        // Cache base values
        baseOxygenRate = oxygenManager.depletionRate;
        baseCarryLimit = trashCollection.carryLimit;
    }

    public void RefreshEquippedTools()
    {
        equippedTools = GameDataManager.GetSelectedTools();
    
        if (equippedTools == null || equippedTools.Count == 0)
        {
            AssignDefaultTool();
            equippedTools = GameDataManager.GetSelectedTools();
        }

        EquipTool(0);
        UpdateInventoryUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryOpen = !inventoryOpen;
            inventoryPanel.SetActive(inventoryOpen);
        }

        if (inventoryOpen)
        {
            for (int i = 0; i < equippedTools.Count; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    EquipTool(i);
                    break;
                }
            }
        }
    }

    public void EquipTool(int index)
    {
        currentToolIndex = index;
        Tool currentTool = equippedTools[currentToolIndex];
        Debug.Log("Equipped: " + currentTool.name);

        playerController.SetSpeedMultiplier(currentTool.speedMultiplier);
        oxygenManager.SetDepletionRate(baseOxygenRate + currentTool.oxygenDepletionRate);
        trashCollection.SetCarryLimit(baseCarryLimit + currentTool.carryCapacityModifier);
        
        HighlightEquippedSlot();
    }
    
    void UpdateInventoryUI()
    {
        for (int i = 0; i < toolIcons.Count; i++)
        {
            if (i < equippedTools.Count)
            {
                toolIcons[i].sprite = equippedTools[i].image;
                toolIcons[i].enabled = true;
            }
            else
            {
                toolIcons[i].enabled = false;
            }
        }

        HighlightEquippedSlot();
    }

    void HighlightEquippedSlot()
    {
        for (int i = 0; i < slotBackgrounds.Count; i++)
        {
            slotBackgrounds[i].color = (i == currentToolIndex) ? equippedSlotColor : defaultSlotColor;
        }
    }
    
    private void AssignDefaultTool()
    {
        if (toolDatabase == null)
        {
            Debug.LogError("ToolStoreDatabase is not assigned in ToolInventoryManager.");
            return;
        }

        List<int> defaultToolIndices = new List<int> { 0 }; // Default tool index is 0
        GameDataManager.SetSelectedTools(defaultToolIndices);
        
        Debug.Log("Default tool assigned and saved.");
    }
}
