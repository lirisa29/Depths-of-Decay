using UnityEngine;
using UnityEngine.UI;

public class ScreenSettings : MonoBehaviour
{
    public Toggle fullscreenToggle;

    void Start()
    {
        // Load saved setting (default to 1/true if nothing saved)
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        // Apply saved fullscreen setting
        Screen.fullScreen = isFullscreen;

        // Sync toggle with saved state
        fullscreenToggle.isOn = isFullscreen;

        // Add listener
        fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
    }

    void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        // Save setting
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}
