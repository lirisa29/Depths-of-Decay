using UnityEngine;

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

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        gameUI = FindFirstObjectByType<GameUI>();
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
    }

    private void CollectTrash(TrashItem trash)
    {
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
        }
    }

    public void DepositTrash()
    {
        gameUI?.HideDepositButton();
        gameUI?.SetCarryTextColor(Color.white);
        
        totalTrashDeposited += carriedTrashCount;
        carriedTrashCount = 0;
        playerController.ResetSpeed();

        gameUI?.UpdateTrashDepositedText(totalTrashDeposited, collectionGoal);
        gameUI?.UpdateTrashCarryingText(carriedTrashCount, carryLimit);

        if (totalTrashDeposited >= collectionGoal)
        {
            gameUI?.ShowWinScreen();
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

        if (carriedTrashCount != 0)
        {
            gameUI?.ShowDepositButton();
        }
    }

    public void NotifyExitedBase()
    {
        gameUI?.HideDepositButton();

        if (projectedTotal >= collectionGoal)
        {
            gameUI?.ShowCarryStatus("All Trash Collected! Go back to Base");
        }
        else if (carriedTrashCount >= carryLimit)
        {
            gameUI?.ShowCarryStatus("Carry Limit Reached! Go back to Base");
        }
    }
}
