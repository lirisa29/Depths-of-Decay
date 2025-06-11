using UnityEngine;
using TMPro;

public class TrashInfoPopup : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI weightText;
    public TextMeshProUGUI speedImpactText;

    public void SetInfo(string trashName, float weightKg, float speedDebuffPercent)
    {
        if (nameText) nameText.text = trashName;
        if (weightText) weightText.text = $"Weight: {weightKg:0.00} kg";
        if (speedImpactText) speedImpactText.text = $"Speed Impact: -{speedDebuffPercent:0}%";
    }
}
