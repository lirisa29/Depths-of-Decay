using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

//Shop Data Holder
[System.Serializable] public class ToolStoreData
{
	public List<int> purchasedToolsIndexes = new List<int> ();
}
//Player Data Holder
[System.Serializable] public class PlayerData
{
	public int points = 0;
	public List<int> selectedToolIndices = new List<int>();
	
	public int highestUnlockedLevel = 1; // Start with level 1 unlocked
	public List<int> completedLevels = new List<int>();
}

public static class GameDataManager
{
	static PlayerData playerData = new PlayerData ();
	static ToolStoreData toolStoreData = new ToolStoreData ();
	
	public static ToolStoreDatabase toolDatabase;

	static Tool selectedTool;

	static GameDataManager ()
	{
		LoadPlayerData ();
		LoadToolStoreData ();
	}

	//Player Data Methods -----------------------------------------------------------------------------
	
	public static int GetAllowedToolSlots()
	{
		int currentLevel = GetCurrentLevelIndex();

		if (currentLevel <= 2) return 2;
		else if (currentLevel == 3) return 3;
		else return 4; // Extend as you wish
	}
	
	public static List<Tool> GetSelectedTools()
	{
		var tools = new List<Tool>();
		foreach (var index in playerData.selectedToolIndices)
		{
			tools.Add(toolDatabase.GetTool(index));
		}
		return tools;
	}

	public static void SetSelectedTools(List<int> indices)
	{
		playerData.selectedToolIndices = new List<int>(indices);
		SavePlayerData();
	}

	public static List<int> GetSelectedToolIndices()
	{
		return new List<int>(playerData.selectedToolIndices);
	}

	public static int GetSelectedToolIndex()
	{
		return playerData.selectedToolIndices.Count > 0 ? playerData.selectedToolIndices[0] : 0;
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
	
	public static bool HasLevelBeenRewarded(int level)
	{
		return playerData.completedLevels.Contains(level);
	}

	public static void MarkLevelAsRewarded(int level)
	{
		if (!playerData.completedLevels.Contains(level))
		{
			playerData.completedLevels.Add(level);
			SavePlayerData();
		}
	}
	
	public static int GetCurrentLevelIndex()
	{
		return SceneManager.GetActiveScene().buildIndex;
	}
	
	public static int GetHighestUnlockedLevel()
	{
		return playerData.highestUnlockedLevel;
	}

	public static void SetHighestUnlockedLevel(int level)
	{
		if (level > playerData.highestUnlockedLevel)
		{
			playerData.highestUnlockedLevel = level;
			SavePlayerData();
		}
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
