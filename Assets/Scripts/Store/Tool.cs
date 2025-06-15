using UnityEngine;

public enum ToolType { None, Net, Fins, OxygenTank }

[System.Serializable]
public struct Tool
{
    public Sprite image;
    public string name;
    public string pros;
    public string cons;
    public int price;
    
    public bool isPurchased;
}
