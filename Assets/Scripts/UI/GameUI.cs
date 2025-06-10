using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI trashCountText;
    public TextMeshProUGUI speedText;

    public void UpdateTrash(int carried, int deposited, int goal)
    {
        if (trashCountText)
            trashCountText.text = $"Carrying: {carried}/5 | Deposited: {deposited}/{goal}";
    }

    public void UpdateSpeed(float speed)
    {
        if (speedText)
            speedText.text = $"Current Speed: {speed:F2}";
    }
}
