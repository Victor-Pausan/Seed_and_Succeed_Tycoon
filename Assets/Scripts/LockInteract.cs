using UnityEngine;

public class LockInteract : MonoBehaviour
{
    public float unlockCost = 50f; // Cost to unlock (set in Inspector for each lock)
    public GameObject menu;
    public GameObject carToUnlock; // The specific car this lock will reveal
    private float balance;

    // Enum to identify which car this lock controls (optional, for clarity)
    public enum CarType { Taxi, RS6, Tesla, Porsche, BMW, Lambo }
    public CarType carType;

    void Start()
    {
        // Ensure the car starts hidden
        if (carToUnlock != null)
        {
            carToUnlock.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        // Get current balance
        balance = GameManager.suprBalance;

        // Check if the player can afford to unlock
        if (unlockCost > balance)
        {
            Debug.Log($"Not enough balance! Need: {unlockCost}, Have: {balance}");
            return;
        }

        // Check if lock is under menu
        if (gameObject.CompareTag("LockUnderMenu"))
        {
            if (menu != null && menu.activeSelf)
                return;
        }

        // Unlock the car and remove the lock
        UnlockCar();
    }

    private void UnlockCar()
    {
        // Deactivate the lock
        gameObject.SetActive(false);

        // Activate the corresponding car
        if (carToUnlock != null)
        {
            carToUnlock.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No car assigned to unlock!");
            return;
        }

        // Deduct the cost from balance
        GameManager.suprBalance -= unlockCost;
        Debug.Log($"{carType} unlocked! New balance: {GameManager.suprBalance}");
    }
}