using UnityEngine;

[CreateAssetMenu(fileName = "Store Database", menuName = "Store/Store Database")]
public class ToolStoreDatabase : ScriptableObject
{
    public Tool[] tools;
    public int ToolsCount{ get { return tools.Length; } }

    public void PurchaseTool(int index)
    {
        tools[index].isPurchased = true;
    }

    public Tool GetTool(int index)
    {
        return tools[index];
    }
}
