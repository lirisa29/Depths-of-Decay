using UnityEngine;
using System.Collections.Generic;

public class ToolInventoryManager : MonoBehaviour
{
    private List<Tool> equippedTools;
    private int currentToolIndex = 0;
    private bool inventoryOpen = false;

    void Start()
    {
        equippedTools = GameDataManager.GetSelectedTools();
        EquipTool(0); // default tool
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryOpen = !inventoryOpen;
            // Show/hide inventory UI if needed
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

    void EquipTool(int index)
    {
        currentToolIndex = index;
        Tool currentTool = equippedTools[currentToolIndex];
        Debug.Log("Equipped: " + currentTool.name);

        // Add logic to update your player's current tool functionality
    }
}
