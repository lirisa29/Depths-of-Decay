using UnityEngine;

public class LevelStartManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject toolSelectionPanel;
    [SerializeField] private GameObject gameplayObjects; // Root of player/enemies/scripts etc

    private bool isReplayLevel1;

    void Start()
    {
        Time.timeScale = 0f; // Pause the game
        gameplayObjects.SetActive(false);

        int currentLevel = GameDataManager.GetCurrentLevelIndex();
        isReplayLevel1 = (currentLevel == 1 && GameDataManager.GetHighestUnlockedLevel() > 1);

        dialoguePanel.SetActive(true);
        toolSelectionPanel.SetActive(false);
    }

    public void HandleDialogueFinished()
    {
        dialoguePanel.SetActive(false);

        int currentLevel = GameDataManager.GetCurrentLevelIndex();
        if (currentLevel >= 2 || isReplayLevel1)
        {
            toolSelectionPanel.SetActive(true);
        }
        else
        {
            StartGameplay();
        }
    }

    public void OnToolSelectionConfirmed()
    {
        toolSelectionPanel.SetActive(false);
        StartGameplay();
    }

    private void StartGameplay()
    {
        gameplayObjects.SetActive(true);
        Time.timeScale = 1f;
    }
}
