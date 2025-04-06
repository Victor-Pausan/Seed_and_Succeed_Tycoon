using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BiddingSystem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer targetSprite;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float maxX = 10.0f;
    
    private Vector3 initialPosition = new Vector3(-395.7486f, -170.2791f, 1.67039f);
    private float currentX = 0.0f;
    private bool isMoving = false;
    
    public TMP_InputField bidInputField;
    public Button placeBidButton;
    public TextMeshProUGUI resultText;

    [SerializeField] private TextMeshProUGUI[] npcNameTexts = new TextMeshProUGUI[4];
    [SerializeField] private TextMeshProUGUI[] npcBidTexts = new TextMeshProUGUI[4];

    private string[] npcNames = new string[]
    {
        "Alex", "Bella", "Charlie", "Diana", "Ethan",
        "Fiona", "George", "Hannah", "Isaac", "Julia",
        "Kevin", "Lily", "Mason", "Nora", "Oliver"
    };

    private float bidCooldown = 300f; // 5 minutes in seconds
    private float timeRemaining = 0f; // Time left until next bid
    private bool isCooldownActive = false;

    void Start()
    {
        resultText.text = $"Welcome to the Proof-of-Repayment game! Take the chance to help repay loans by bidding against other players. If your bid is the highest, you will be rewarded with SuperSeeds.";
        
        if (placeBidButton == null)
        {
            Debug.LogError("PlaceBidButton is not assigned in the Inspector!");
            return;
        }

        if (targetSprite == null)
        {
            targetSprite = GetComponent<SpriteRenderer>();
            if (targetSprite == null)
            {
                Debug.LogError("No sprite renderer assigned to BiddingSystem script!");
                return;
            }
        }

        for (int i = 0; i < npcNameTexts.Length; i++)
        {
            if (npcNameTexts[i] != null) npcNameTexts[i].text = "";
            if (npcBidTexts[i] != null) npcBidTexts[i].text = "";
        }
        
        targetSprite.transform.position = initialPosition;
        targetSprite.enabled = true;

        // Initially enable the button if no cooldown
        placeBidButton.interactable = !isCooldownActive;
    }

    void Update()
    {
        if (isMoving)
        {
            currentX += speed * Time.deltaTime;
            float newY = -Mathf.Pow(currentX, 2);
            Vector3 newPosition = new Vector3(
                initialPosition.x + currentX,
                initialPosition.y + newY,
                initialPosition.z
            );
            targetSprite.transform.position = newPosition;

            if (currentX >= maxX)
            {
                isMoving = false;
                Debug.Log("Movement complete - reached max X");
            }
        }

        // Update cooldown timer
        if (isCooldownActive)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                isCooldownActive = false;
                placeBidButton.interactable = true;
                TextMeshProUGUI buttonText = placeBidButton.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null) buttonText.text = "Place Bid";
                resultText.text = "Bid cooldown expired! You can place a new bid now.";
            }
            else
            {
                UpdateResultTextWithTimer();
            }
        }
    }

    public void PlaceBid()
    {
        if (isCooldownActive)
        {
            resultText.text = $"You must wait {FormatTime(timeRemaining)} before placing another bid!";
            return;
        }

        Debug.Log("PlaceBid called with input: " + bidInputField.text);
        
        if (int.TryParse(bidInputField.text, out int playerBid) && playerBid > 0)
        {
            List<string> selectedNames = GetRandomNames(4);
            Dictionary<string, float> npcBids = GenerateNPCBids(playerBid, selectedNames);

            for (int i = 0; i < selectedNames.Count; i++)
            {
                if (npcNameTexts[i] != null) npcNameTexts[i].text = selectedNames[i];
                if (npcBidTexts[i] != null) npcBidTexts[i].text = $"{npcBids[selectedNames[i]]:F2} USDC";
            }

            string winnerName = "Player";
            float highestBid = playerBid;

            foreach (var npcBid in npcBids)
            {
                if (npcBid.Value > highestBid)
                {
                    winnerName = npcBid.Key;
                    highestBid = npcBid.Value;
                }
            }

            if (winnerName == "Player")
            {
                float rewardPercentage = Random.Range(0.10f, 0.15f);
                float rewardAmount = GameManager.suprBalance * rewardPercentage;
                rewardAmount = Mathf.Floor(rewardAmount * 100f) / 100f;
                GameManager.suprBalance += rewardAmount;

                resultText.text = $"Congratulations! Your bid of {playerBid} USDC was the highest! You beat {string.Join(", ", selectedNames)}. Rewarded with {rewardAmount:F2} SuperSeeds. New balance: {GameManager.suprBalance:F2}";
                
                if (targetSprite != null)
                {
                    targetSprite.transform.position = initialPosition;
                    StartMovement();
                }
            }
            else
            {
                resultText.text = $"You lost! {winnerName} placed a higher bid of {highestBid:F2} USDC. Try again later...!";
            }

            // Start cooldown
            StartCooldown();
            DisableBidButton();
        }
        else
        {
            resultText.text = "Please enter a valid bid amount!";
        }
    }

    private void StartCooldown()
    {
        isCooldownActive = true;
        timeRemaining = bidCooldown;
        UpdateResultTextWithTimer();
    }

    private void UpdateResultTextWithTimer()
    {
        string currentText = resultText.text.Split('\n')[0]; // Keep the first line (result)
        resultText.text = $"{currentText}\nNext bid available in: {FormatTime(timeRemaining)}";
    }

    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int secs = Mathf.FloorToInt(seconds % 60);
        return $"{minutes:D2}:{secs:D2}";
    }

    private List<string> GetRandomNames(int count)
    {
        List<string> randomNames = new List<string>();
        List<int> usedIndices = new List<int>();

        while (randomNames.Count < count)
        {
            int index = Random.Range(0, npcNames.Length);
            if (!usedIndices.Contains(index))
            {
                usedIndices.Add(index);
                randomNames.Add(npcNames[index]);
            }
        }

        return randomNames;
    }

    private Dictionary<string, float> GenerateNPCBids(int playerBid, List<string> names)
    {
        Dictionary<string, float> npcBids = new Dictionary<string, float>();

        foreach (string name in names)
        {
            float bidMultiplier = Random.Range(0.7f, 1.1f);
            float npcBid = playerBid * bidMultiplier;
            npcBid = Mathf.Floor(npcBid * 100f) / 100f;
            npcBids.Add(name, npcBid);
            Debug.Log($"{name} bids {npcBid:F2} USDC");
        }

        return npcBids;
    }

    private void DisableBidButton()
    {
        placeBidButton.interactable = false;
        TextMeshProUGUI buttonText = placeBidButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = "Try Again Later";
        }
    }

    public void StartMovement()
    {
        targetSprite.transform.position = initialPosition;
        currentX = 0.0f;
        isMoving = true;
        Debug.Log("Movement started!");
    }
}