using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrashCollection : MonoBehaviour
{
    public float speedDebuffPerKg = 0.1f;
    public int carryLimit = 5;
    public int collectionGoal = 20;

    private int carriedTrashCount = 0;
    private int totalTrashDeposited = 0;
    private int projectedTotal;

    private TrashItem nearbyTrash;
    private PlayerController playerController;
    private GameUI gameUI;
    private OxygenManager oxygenManager;

    [SerializeField] private TextMeshProUGUI levelPointsAwardedText;
    
    private int baseCarryLimit;
    
    [SerializeField] private GameObject milestoneImage;
    
    private AudioManager audioManager;

    private bool enteredBase;
    private float levelStartTime;

    void Start()
    {
        levelStartTime = Time.time;
        audioManager = FindFirstObjectByType<AudioManager>();
        
        playerController = GetComponent<PlayerController>();
        gameUI = FindFirstObjectByType<GameUI>();
        oxygenManager = FindFirstObjectByType<OxygenManager>();
        
        baseCarryLimit = carryLimit;
        
        gameUI?.UpdateTrashCarryingText(carriedTrashCount, carryLimit);
        gameUI?.UpdateTrashDepositedText(totalTrashDeposited, collectionGoal);
    }

    void Update()
    {
        // Pickup
        if (nearbyTrash != null && Input.GetKeyDown(KeyCode.E))
        {
            if (carriedTrashCount < carryLimit)
            {
                CollectTrash(nearbyTrash);
                nearbyTrash = null;
                
                if (carriedTrashCount >= carryLimit)
                {
                    gameUI.SetCarryTextColor(Color.red);
                    gameUI?.ShowCarryStatus("Carry Limit Reached! Go back to Base");
                }
            }
        }
        
        if (enteredBase)
        {
            if (Input.GetKeyDown(KeyCode.Q) && carriedTrashCount != 0)
            {
                DepositTrash();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameUI.TogglePause();
        }
    }

    private void CollectTrash(TrashItem trash)
    {
        audioManager.PlaySound("SFX_TrashPickup");
        carriedTrashCount++;
        float debuff = trash.GetWeight() * speedDebuffPerKg;
        playerController.ApplySpeedDebuff(debuff);

        Destroy(trash.gameObject);

        gameUI?.UpdateTrashCarryingText(carriedTrashCount, carryLimit);
        
        // Check if this collection completes the goal
        projectedTotal = totalTrashDeposited + carriedTrashCount;
        if (projectedTotal >= collectionGoal)
        {
            gameUI?.ShowCarryStatus("All Trash Collected! Go back to Base");
            
            int levelIndex = GameDataManager.GetCurrentLevelIndex();

            switch (levelIndex)
            {
                case 1: // Level 1
                    audioManager.PlayBackgroundMusic("BGM_Level1Win");
                    break;
                case 2: // Level 2
                    audioManager.PlayBackgroundMusic("BGM_Level2Win");
                    break;
                case 3: // Level 3
                    audioManager.PlayBackgroundMusic("BGM_Level3Win");
                    break;
                default:
                    audioManager.PlayBackgroundMusic("BGM_MainMenu"); // fallback/default music
                    break;
            }
        }
    }

    public void DepositTrash()
    {
        audioManager.PlaySound("SFX_TrashDeposit");
        gameUI?.HideDepositButton();
        gameUI?.SetCarryTextColor(Color.white);
        
        for (int i = 0; i < carriedTrashCount; i++)
        {
            oxygenManager.RefillOxygen(20);
            Debug.Log("Oxygen Refilled");
        }
        
        totalTrashDeposited += carriedTrashCount;
        
        UpdateMilestoneSpriteOpacity();
        
        carriedTrashCount = 0;
        playerController.ResetDebuffOnly();

        gameUI?.UpdateTrashDepositedText(totalTrashDeposited, collectionGoal);
        gameUI?.UpdateTrashCarryingText(carriedTrashCount, carryLimit);

        if (totalTrashDeposited >= collectionGoal)
        {
            gameUI?.ShowWinScreen();

            audioManager.PlaySound("SFX_MissionWin");
            
            int currentLevel = GameDataManager.GetCurrentLevelIndex();
            
            float currentTime = Time.time - levelStartTime;
            int levelIndex = GameDataManager.GetCurrentLevelIndex();

            GameDataManager.SaveBestTime(levelIndex, currentTime);

            float bestTime = GameDataManager.GetBestTime(levelIndex);

            if (GameDataManager.HasLevelBeenRewarded(levelIndex))
            {
                gameUI?.ShowWinTimes(currentTime, bestTime);
            }

            if (!GameDataManager.HasLevelBeenRewarded(currentLevel))
            {
                int pointsAwarded = 75 + (currentLevel - 1) * 25; // Level 1 = 75, Level 2 = 100, etc.
                levelPointsAwardedText.text = pointsAwarded.ToString();
                GameDataManager.AddPoints(pointsAwarded);
                GameDataManager.MarkLevelAsRewarded(currentLevel);
                GameSharedUI.Instance.UpdatePointsUIText(); // Refresh UI
            }

            if (currentLevel >= GameDataManager.GetHighestUnlockedLevel())
            {
                GameDataManager.SetHighestUnlockedLevel(currentLevel + 1);
            }
        }
    }
    
    public void NotifyNearTrash(TrashItem trash)
    {
        nearbyTrash = trash;

        if (nearbyTrash != null)
        {
            string name = nearbyTrash.GetTrashName();
            float weight = nearbyTrash.GetWeight();
            float debuffPercent = weight * speedDebuffPerKg * 100f;

            Vector3 popupPosition = playerController.transform.position + new Vector3(1f, 1.5f, 0f);
            gameUI?.ShowTrashInfoPopup(name, weight, debuffPercent, popupPosition);
        }
    }
    
    public void NotifyLeftTrash(TrashItem trash)
    {
        if (nearbyTrash == trash)
        {
            nearbyTrash = null;
            gameUI?.HideTrashInfoPopup();
        }
    }
    
    public void NotifyEnteredBase()
    {
        gameUI?.HideCarryingStatus();

        enteredBase = true;

        if (carriedTrashCount != 0)
        {
            gameUI?.ShowDepositButton();
        }
    }

    public void NotifyExitedBase()
    {
        gameUI?.HideDepositButton();
        
        enteredBase = false;

        if (projectedTotal >= collectionGoal)
        {
            gameUI?.ShowCarryStatus("All Trash Collected! Go back to Base");
        }
        else if (carriedTrashCount >= carryLimit)
        {
            gameUI?.ShowCarryStatus("Carry Limit Reached! Go back to Base");
        }
    }
    
    public void SetCarryLimit(int newLimit)
    {
        carryLimit = newLimit;
        gameUI?.UpdateTrashCarryingText(carriedTrashCount, carryLimit);
    }
    
    private void UpdateMilestoneSpriteOpacity()
    {
        if (milestoneImage == null) return;

        SpriteRenderer sr = milestoneImage.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Calculate progress between 0 (0%) and 1 (100%)
        float progress = Mathf.Clamp01((float)totalTrashDeposited / collectionGoal);

        // Calculate alpha as inverse of progress
        float alpha = 1f - progress;

        // Apply new alpha to the sprite's color
        Color color = sr.color;
        color.a = alpha;
        sr.color = color;
    }
}
