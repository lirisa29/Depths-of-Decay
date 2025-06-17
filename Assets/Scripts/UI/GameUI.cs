using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI trashCarryingText;
    public TextMeshProUGUI trashDepositedText;
    public TextMeshProUGUI carryingStatusText;
    public TextMeshProUGUI deposit;
    public GameObject trashInfoPopupPrefab;
    private GameObject activeTrashPopup;
    public Transform canvasTransform;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject ingameUI;
    public GameObject pauseMenu;
    public GameObject pointsPanel;
    public GameObject bestTimePanel;
    private bool isPaused;
    private bool isGameOver;
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI bestTimeText;

    public void UpdateTrashCarryingText(int carried, int limit)
    {
        if (trashCarryingText)
            trashCarryingText.text = $"CARRYING: {carried} / {limit}";
    }

    public void UpdateTrashDepositedText(int deposited, int goal)
    {
        if (trashCarryingText)
            trashDepositedText.text = $"DEPOSITED: {deposited} / {goal}";
    }
    
    public void ShowCarryStatus(string message)
    {
        if (carryingStatusText)
        {
            carryingStatusText.gameObject.SetActive(true);
            carryingStatusText.text = message;
        }
    }

    public void HideCarryingStatus()
    {
        if (carryingStatusText)
            carryingStatusText.gameObject.SetActive(false);
    }

    public void ShowDepositButton()
    {
        if (deposit)
            deposit.gameObject.SetActive(true);
    }

    public void HideDepositButton()
    {
        if (deposit)
            deposit.gameObject.SetActive(false);
    }
    
    public void SetCarryTextColor(Color color)
    {
        if (trashCarryingText)
            trashCarryingText.color = color;
    }
    
    public void ShowTrashInfoPopup(string trashName, float weight, float speedDebuffPercent, Vector3 worldPosition)
    {
        if (trashInfoPopupPrefab == null) return;

        if (activeTrashPopup != null)
            Destroy(activeTrashPopup);

        activeTrashPopup = Instantiate(trashInfoPopupPrefab, canvasTransform);
    
        TrashInfoPopup popupScript = activeTrashPopup.GetComponent<TrashInfoPopup>();
        if (popupScript != null)
        {
            popupScript.SetInfo(trashName, weight, speedDebuffPercent);
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        activeTrashPopup.transform.position = screenPos;
    }

    public void HideTrashInfoPopup()
    {
        if (activeTrashPopup != null)
            Destroy(activeTrashPopup);
    }

    public void ShowWinScreen()
    {
        Time.timeScale = 0;
        isGameOver = true;
        ingameUI.SetActive(false);
        HideTrashInfoPopup();
        winScreen.SetActive(true);
    }

    public void ShowLoseScreen()
    {
        Time.timeScale = 0;
        isGameOver = true;
        ingameUI.SetActive(false);
        HideTrashInfoPopup();
        loseScreen.SetActive(true);
    }

    public void PauseGame()
    {
        if (!isPaused && !isGameOver)
        {
            Time.timeScale = 0;

            isPaused = true;

            ingameUI.SetActive(false);
            HideTrashInfoPopup();
            pauseMenu.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        if (isPaused && !isGameOver)
        {
            Time.timeScale = 1;
            isPaused = false;
            ingameUI.SetActive(true);
            pauseMenu.SetActive(false);
        }
    }
    
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    public void ShowWinTimes(float currentTime, float bestTime)
    {
        pointsPanel.SetActive(false);
        bestTimePanel.SetActive(true);
        
        if (currentTimeText) currentTimeText.text = $"Cleanup Completion Time: {currentTime:F2}s";
        if (bestTimeText)
        {
            if (bestTime == float.MaxValue)
                bestTimeText.text = "Best Time: N/A";
            else
                bestTimeText.text = $"Best Time: {bestTime:F2}s";
        }
    }
}
