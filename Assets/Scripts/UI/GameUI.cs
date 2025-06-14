using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI trashCarryingText;
    public TextMeshProUGUI trashDepositedText;
    public TextMeshProUGUI carryingStatusText;
    public GameObject depositButton;
    public GameObject trashInfoPopupPrefab;
    private GameObject activeTrashPopup;
    public Transform canvasTransform;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject ingameUI;
    public GameObject pauseMenu;
    private bool isPaused;

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
        if (depositButton)
            depositButton.SetActive(true);
    }

    public void HideDepositButton()
    {
        if (depositButton)
            depositButton.SetActive(false);
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
        ingameUI.SetActive(false);
        winScreen.SetActive(true);
    }

    public void ShowLoseScreen()
    {
        Time.timeScale = 0;
        ingameUI.SetActive(false);
        loseScreen.SetActive(true);
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            //audioManager.PlaySound("ButtonClick");
            Time.timeScale = 0;

            isPaused = true;

            ingameUI.SetActive(false);
            pauseMenu.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        ingameUI.SetActive(true);
        pauseMenu.SetActive(false);
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
}
