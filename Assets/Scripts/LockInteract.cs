using UnityEngine;

public class LockInteract : MonoBehaviour
{
    public float unlockCost = 50f; // Cost to unlock (adjust as needed)

    void OnMouseDown()
    {
        // Check if the player can afford to unlock
            gameObject.SetActive(false); // Deactivate the lock
            Debug.Log("Lock unlocked!");
    }
}