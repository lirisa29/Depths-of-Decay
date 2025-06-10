using UnityEngine;

public class TrashCollection : MonoBehaviour
{
    public float speedDebuffPerKg = 0.1f;
    public int carryLimit = 5;
    public int collectionGoal = 20;

    private int carriedTrashCount = 0;
    private int totalTrashDeposited = 0;

    private TrashItem nearbyTrash;
    private PlayerController playerController;
    private GameUI gameUI;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        gameUI = FindFirstObjectByType<GameUI>();
        gameUI?.UpdateTrash(carriedTrashCount, totalTrashDeposited, collectionGoal);
        gameUI?.UpdateSpeed(playerController.currentSpeed);
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
            }
            else
            {
                Debug.Log("Trash carry limit reached!");
            }
        }
    }

    private void CollectTrash(TrashItem trash)
    {
        carriedTrashCount++;
        float debuff = trash.GetWeight() * speedDebuffPerKg;
        playerController.ApplySpeedDebuff(debuff);

        Destroy(trash.gameObject);

        gameUI?.UpdateTrash(carriedTrashCount, totalTrashDeposited, collectionGoal);
        gameUI?.UpdateSpeed(playerController.currentSpeed);
    }

    public void DepositTrash()
    {
        totalTrashDeposited += carriedTrashCount;
        carriedTrashCount = 0;
        playerController.ResetSpeed();

        gameUI?.UpdateTrash(carriedTrashCount, totalTrashDeposited, collectionGoal);
        gameUI?.UpdateSpeed(playerController.currentSpeed);

        if (totalTrashDeposited >= collectionGoal)
        {
            Debug.Log("Goal reached! All trash collected.");
            // Trigger win/next level/celebration here
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trash"))
        {
            nearbyTrash = other.GetComponent<TrashItem>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Trash") && nearbyTrash == other.GetComponent<TrashItem>())
        {
            nearbyTrash = null;
        }
    }
}
