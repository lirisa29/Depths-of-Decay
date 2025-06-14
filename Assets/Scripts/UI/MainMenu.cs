using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu Panels")]
    public GameObject mainMenu;
    public GameObject playPanel;
    public GameObject optionsPanel;
    public GameObject howToPlayPanel;
    public GameObject storePanel;
    
    private ButtonOutlineEffect[] allOutlines;

    void Awake()
    {
        // Get all ButtonOutlineEffect scripts in children (main menu + subpanels)
        allOutlines = FindObjectsByType<ButtonOutlineEffect>(FindObjectsSortMode.None);
    }

    // Open a specific panel (closes others)
    public void OpenPlayPanel()
    {
        mainMenu.SetActive(false);
        playPanel.SetActive(true);
    }

    public void OpenOptionsPanel()
    {
        mainMenu.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OpenHowToPlayPanel()
    {
        mainMenu.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void OpenStorePanel()
    {
        mainMenu.SetActive(false);
        storePanel.SetActive(true);
    }
    
    private void ResetAllOutlines()
    {
        foreach (var outline in allOutlines)
        {
            outline.ResetOutline();
        }
    }

    // Close one specific panel
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        mainMenu.SetActive(true);
        ResetAllOutlines();
    }
}
