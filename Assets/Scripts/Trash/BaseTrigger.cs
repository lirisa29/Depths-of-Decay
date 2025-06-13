using UnityEngine;

public class BaseTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var trashCollector = other.GetComponent<TrashCollection>();
            if (trashCollector != null)
            {
                trashCollector.NotifyEnteredBase();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var trashCollector = other.GetComponent<TrashCollection>();
            if (trashCollector != null)
            {
                trashCollector.NotifyExitedBase();
            }
        }
    }
}
