using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static void LoadScene(int sceneIndex)
    {
        Time.timeScale = 1;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllSounds();
        }

        if (sceneIndex == 0)
        {
            AudioManager.Instance.PlayBackgroundMusic("BGM_MainMenu");
        }
        else
        {
            AudioManager.Instance.PlayBackgroundMusic("BGM_In-Game");
        }

        SceneManager.LoadScene(sceneIndex);
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
