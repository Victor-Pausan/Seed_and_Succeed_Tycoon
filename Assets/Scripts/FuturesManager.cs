using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FuturesManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI balanceText;    
    [SerializeField] private TMP_InputField amountInput;    
    [SerializeField] private Button longButton;             
    [SerializeField] private Button shortButton;           
    [SerializeField] private Image picturePanel;           
    [SerializeField] private TextMeshProUGUI textPanel;    
    [SerializeField] private Sprite graphUp;               
    [SerializeField] private Sprite graphDown;             

    private float currentBalance;  // Remove direct initialization

    void Start()
    {
        // Use GameManager's suprBalance instead of direct initialization
        currentBalance = GameManager.suprBalance;
        
        UpdateBalanceText();
        
        longButton.onClick.AddListener(() => PlaceBet(true));
        shortButton.onClick.AddListener(() => PlaceBet(false));
    }

    void UpdateBalanceText()
    {
        balanceText.text = $"SUPR Balance: {currentBalance:F2}";
        // Sync the balance back to GameManager
        GameManager.suprBalance = currentBalance;
    }

    void PlaceBet(bool isLong)
    {
        // Get bet amount from input
        if (!float.TryParse(amountInput.text, out float betAmount))
        {
            textPanel.text = "Please enter a valid amount!";
            return;
        }

        // Validate bet amount
        if (betAmount <= 0)
        {
            textPanel.text = "Bet amount must be greater than 0!";
            return;
        }

        if (betAmount > currentBalance)
        {
            textPanel.text = "Insufficient balance!";
            return;
        }

        // Randomly determine market direction (50/50 chance)
        bool marketWentUp = Random.value > 0.5f;
        
        // Determine if player won
        bool playerWon = (isLong && marketWentUp) || (!isLong && !marketWentUp);

        // Update balance
        if (playerWon)
        {
            currentBalance += betAmount;
            textPanel.text = isLong ? 
                $"Market went LONG! You won {betAmount:F2} SUPR" : 
                $"Market went SHORT! You won {betAmount:F2} SUPR";
        }
        else
        {
            currentBalance -= betAmount;
            textPanel.text = isLong ? 
                $"Market went SHORT! You lost {betAmount:F2} SUPR" : 
                $"Market went LONG! You lost {betAmount:F2} SUPR";
        }

        // Update UI and sync with GameManager
        UpdateBalanceText();
        picturePanel.sprite = marketWentUp ? graphUp : graphDown;
        
        // Clear input field
        amountInput.text = "";
    }

    void Update()
    {
        // Optional: Continuously sync balance with GameManager
        // This ensures balance stays in sync even if changed elsewhere
        if (Mathf.Abs(currentBalance - GameManager.suprBalance) > 0.01f)
        {
            currentBalance = GameManager.suprBalance;
            UpdateBalanceText();
        }
    }
}