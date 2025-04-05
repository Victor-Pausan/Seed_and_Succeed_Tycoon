using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BiddingSystem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer targetSprite; // Reference to the sprite
    [SerializeField] private float speed = 5.0f; // Increased speed for more visible movement
    [SerializeField] private float maxX = 10.0f; // Maximum x value to reach
    
    private Vector3 initialPosition = new Vector3(-395.7486f, -170.2791f, 1.67039f); // Specified initial position
    private float currentX = 0.0f;
    private bool isMoving = false;
    
    public TMP_InputField bidInputField; // Reference to the TMP input field
    public Button placeBidButton; // Reference to the place bid button
    public TextMeshProUGUI resultText; // Reference to the TextMeshPro text

    void Start()
    {
        resultText.text = $"Welcome to the Proof-of-Repayment game! Take the chance to help the repay of loans by bidding against other players. If your bid is the highest, you will be rewarded with SuperSeeds.";
        
        // Check if button reference is assigned
        if (placeBidButton == null)
        {
            Debug.LogError("PlaceBidButton is not assigned in the Inspector!");
            return;
        }

        // Check if sprite reference is assigned
        if (targetSprite == null)
        {
            targetSprite = GetComponent<SpriteRenderer>();
            if (targetSprite == null)
            {
                Debug.LogError("No sprite renderer assigned to BiddingSystem script!");
                return;
            }
        }
        
        Debug.Log("Sprite reference found: " + (targetSprite != null));
        
        // Set sprite to initial position
        targetSprite.transform.position = initialPosition;
        Debug.Log("Initial position set to: " + initialPosition);
        
        // Make sure the sprite is visible
        targetSprite.enabled = true;
    }

    public void PlaceBid()
    {
        Debug.Log("PlaceBid called with input: " + bidInputField.text);
        
        if (int.TryParse(bidInputField.text, out int bidAmount) && bidAmount > 0)
        {
            bool isHighestBid = Random.value > 0.5f; // 50% chance of winning
            Debug.Log("Bid result: " + (isHighestBid ? "Won" : "Lost"));
            
            if (isHighestBid)
            {
                // Calculate random reward (10% to 15% of current balance)
                float rewardPercentage = Random.Range(0.10f, 0.15f); // 10% to 15%
                float rewardAmount = GameManager.suprBalance * rewardPercentage;
                GameManager.suprBalance += rewardAmount; // Increase balance

                // Update result text with the reward amount
                resultText.text = $"Congratulations! Your bid was the highest! Thank you for contributing to the Proof-of-Repayment system! You have been rewarded with {rewardAmount:F2} SuperSeeds. New balance: {GameManager.suprBalance:F2}";

                if (targetSprite != null)
                {
                    // Set sprite to initial position
                    targetSprite.transform.position = initialPosition;
                    Debug.Log("Starting movement from position: " + initialPosition);
                    StartMovement();
                }
            }
            else
            {
                resultText.text = "You lost! Your bid amount has been returned. Try again later!";
            }

            // Disable the button and change its text
            DisableBidButton();
        }
        else
        {
            resultText.text = "Please enter a valid bid amount!";
        }
    }

    private void DisableBidButton()
    {
        placeBidButton.interactable = false; // Disable the button
        TextMeshProUGUI buttonText = placeBidButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = "Try Again Tomorrow"; // Change button text
        }
        else
        {
            Debug.LogWarning("No TextMeshProUGUI component found on the button!");
        }
    }

    void Update()
    {
        if (isMoving)
        {
            // Increment x based on speed and time
            currentX += speed * Time.deltaTime;
            
            // Calculate new position based on the equation y = -x²
            float newY = -Mathf.Pow(currentX, 2);
            
            // Set the new position
            Vector3 newPosition = new Vector3(
                initialPosition.x + currentX,
                initialPosition.y + newY,
                initialPosition.z
            );
            
            targetSprite.transform.position = newPosition;
            
            // Stop when we reach the maximum x value
            if (currentX >= maxX)
            {
                isMoving = false;
                Debug.Log("Movement complete - reached max X");
            }
        }
    }
    
    public void StartMovement()
    {
        // Reset position and start moving
        targetSprite.transform.position = initialPosition;
        currentX = 0.0f;
        isMoving = true;
        Debug.Log("Movement started!");
    }
    
    // Alternative method using coroutine for smoother movement
    public void StartSmoothMovement()
    {
        StopAllCoroutines();
        targetSprite.transform.position = initialPosition;
        StartCoroutine(MoveAlongParabola());
        Debug.Log("Smooth movement started!");
    }
    
    private IEnumerator MoveAlongParabola()
    {
        currentX = 0.0f;
        
        while (currentX <= maxX)
        {
            // Calculate new position based on the equation y = -x²
            float newY = -Mathf.Pow(currentX, 2);
            
            // Set the new position
            targetSprite.transform.position = new Vector3(
                initialPosition.x + currentX,
                initialPosition.y + newY,
                initialPosition.z
            );
            
            // Increment x based on speed and time
            currentX += speed * Time.deltaTime;
            
            yield return null;
        }
        
        Debug.Log("Coroutine movement complete");
    }
}