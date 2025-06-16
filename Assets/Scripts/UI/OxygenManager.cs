using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OxygenManager : MonoBehaviour
{
    public Image oxygenBarFill;
    public float maxOxygen = 100f;
    public float depletionRate = 5f; // oxygen units per second

    private float currentOxygen;
    private bool isDepleting = true;
    
    public Color normalColor = new Color(0.27f, 0.46f, 0.57f); // #457691
    public Color lowOxygenColor = Color.red;
    public float lowOxygenThreshold = 0.2f;
    
    public TextMeshProUGUI oxygenLabel; // drag your "OXYGEN" text into this in the Inspector

    public Color normalTextColor = Color.white;
    
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.05f;

    private Vector3 originalTextScale;
    private bool isPulsing = false;
    
    private GameUI gameUI;

    void Start()
    {
        gameUI = FindFirstObjectByType<GameUI>();
        currentOxygen = maxOxygen;
        originalTextScale = oxygenLabel.rectTransform.localScale;
        UpdateOxygenUI();
    }

    void Update()
    {
        if (isDepleting)
        {
            currentOxygen -= depletionRate * Time.deltaTime;
            currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);
            UpdateOxygenUI();

            if (currentOxygen <= 0)
            {
                GameOver();
            }
        }
    }

    void UpdateOxygenUI()
    {
        if (oxygenBarFill)
        {
            float fill = currentOxygen / maxOxygen;
            oxygenBarFill.fillAmount = fill;

            // Change color based on oxygen level
            if (fill <= lowOxygenThreshold)
            {
                oxygenBarFill.color = lowOxygenColor;
                PulseText();
            }
            else
            {
                oxygenBarFill.color = normalColor;
                StopPulse();
            }
        }
    }
    
    void PulseText()
    {
        if (!isPulsing)
            isPulsing = true;

        // Scale pulsing
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        oxygenLabel.rectTransform.localScale = originalTextScale * scale;

        // Color change
        oxygenLabel.color = lowOxygenColor;
    }

    void StopPulse()
    {
        if (isPulsing)
        {
            oxygenLabel.rectTransform.localScale = originalTextScale;
            isPulsing = false;
        }
        
        oxygenLabel.color = normalTextColor;
    }

    void GameOver()
    {
        isDepleting = false;
        gameUI?.ShowLoseScreen();
    }

    public void RefillOxygen(float amount)
    {
        currentOxygen = Mathf.Min(currentOxygen + amount, maxOxygen);
        UpdateOxygenUI();
    }
    
    public void SetDepletionRate(float newRate)
    {
        depletionRate = newRate;
    }
}
