using System.Collections.Generic;
using UnityEngine.Serialization;

//Shop Data Holder
[System.Serializable] public class ToolStoreData
{
	public List<int> purchasedToolsIndexes = new List<int> ();
}
//Player Data Holder
[System.Serializable] public class PlayerData
{
	public int points = 0;
	public int selectedToolIndex = 0;
}

public static class GameDataManager
{
	static PlayerData playerData = new PlayerData ();
	static ToolStoreData toolStoreData = new ToolStoreData ();

	static Tool selectedTool;

	static GameDataManager ()
	{
		LoadPlayerData ();
		LoadToolStoreData ();
	}

	//Player Data Methods -----------------------------------------------------------------------------
	public static Tool GetSelectedTool ()
	{
		return selectedTool;
	}

	public static void SetSelectedTool (Tool tool, int index)
	{
		selectedTool = tool;
		playerData.selectedToolIndex = index;
		SavePlayerData ();
	}

	public static int GetSelectedToolIndex ()
	{
		return playerData.selectedToolIndex;
	}

	public static int GetPoints ()
	{
		return playerData.points;
	}

	public static void AddPoints (int amount)
	{
		playerData.points += amount;
		SavePlayerData ();
	}

	public static bool CanSpendPoints (int amount)
	{
		return (playerData.points >= amount);
	}

	public static void SpendPoints (int amount)
	{
		playerData.points -= amount;
		SavePlayerData ();
	}

	static void LoadPlayerData ()
	{
		playerData = BinarySerializer.Load<PlayerData> ("player-data.txt");
		UnityEngine.Debug.Log ("<color=green>[PlayerData] Loaded.</color>");
	}

	static void SavePlayerData ()
	{
		BinarySerializer.Save (playerData, "player-data.txt");
		UnityEngine.Debug.Log ("<color=magenta>[PlayerData] Saved.</color>");
	}

	//Tool Store Data Methods -----------------------------------------------------------------------------
	public static void AddPurchasedTool (int toolIndex)
	{
		toolStoreData.purchasedToolsIndexes.Add (toolIndex);
		SaveToolStoreData ();
	}

	public static List<int> GetAllPurchasedTools ()
	{
		return toolStoreData.purchasedToolsIndexes;
	}

	public static int GetPurchasedTools (int index)
	{
		return toolStoreData.purchasedToolsIndexes [index];
	}

	static void LoadToolStoreData ()
	{
		toolStoreData = BinarySerializer.Load<ToolStoreData> ("tool-store-data.txt");
		UnityEngine.Debug.Log ("<color=green>[ToolStoreData] Loaded.</color>");
	}

	static void SaveToolStoreData ()
	{
		BinarySerializer.Save (toolStoreData, "tool-store-data.txt");
		UnityEngine.Debug.Log ("<color=magenta>[ToolStoreData] Saved.</color>");
	}
}
