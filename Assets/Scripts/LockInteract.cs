using UnityEngine;

public class LockInteract : MonoBehaviour
{
    public float unlockCost = 50f; // Cost to unlock (adjust as needed)
    public GameObject menu;
    public float balance;

    void OnMouseDown()
    {
        // Check if the player can afford to unlock
        balance = GameManager.suprBalance;
        if (unlockCost > balance) return;
        // Check if lock is under menu
        if (gameObject.CompareTag("LockUnderMenu"))
        {
            if (menu != null && menu.activeSelf) 
                return;    
        }
        gameObject.SetActive(false); // Deactivate the lock
        Debug.Log("Lock unlocked!");
    }
}