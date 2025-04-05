using UnityEngine;
using TMPro;

public class BalanceDisplay : MonoBehaviour
{
    public TextMeshProUGUI balanceText; // Reference to the TextMeshPro UI element
    private float lastBalance; // To track changes in balance

    void Start()
    {
        // Ensure the TextMeshPro component is assigned
        if (balanceText == null)
        {
            Debug.LogError("BalanceText is not assigned in the Inspector!");
            return;
        }

        // Initial update
        UpdateBalanceDisplay();
        lastBalance = GameManager.suprBalance;
    }

    void Update()
    {
        // Check if balance has changed
        if (GameManager.suprBalance != lastBalance)
        {
            UpdateBalanceDisplay();
            lastBalance = GameManager.suprBalance;
        }
    }

    private void UpdateBalanceDisplay()
    {
        // Update the text with the current balance
        balanceText.text = $"Supr Balance: {GameManager.suprBalance:F2}";
    }
}