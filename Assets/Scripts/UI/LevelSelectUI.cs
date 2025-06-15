using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelectUI : MonoBehaviour
{
    public List<Button> levelButtons;
    public Sprite lockedSprite;
    public Sprite currentSprite;
    public Sprite unlockedSprite;

    void Start()
    {
        Debug.Log(Application.persistentDataPath+"/GameData/");
        
        int highestUnlocked = GameDataManager.GetHighestUnlockedLevel();

        for (int i = 0; i < levelButtons.Count; i++)
        {
            Button btn = levelButtons[i];
            Image img = btn.GetComponent<Image>();

            if (i + 1 < highestUnlocked)
            {
                img.sprite = unlockedSprite;
                btn.interactable = true;
            }
            else if (i + 1 == highestUnlocked)
            {
                img.sprite = currentSprite;
                btn.interactable = true;
            }
            else
            {
                img.sprite = lockedSprite;
                btn.interactable = false;
            }
        }
    }
}
