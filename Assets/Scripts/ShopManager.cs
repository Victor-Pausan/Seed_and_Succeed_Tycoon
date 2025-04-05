using UnityEngine;
using UnityEngine.UI; // For Button component
using TMPro; // For TextMeshProUGUI component

public class ShopManager : MonoBehaviour
{
    // Reference to GameObjects in the scene
    public GameObject nftPoster;
    public GameObject extendedRoom;
    public GameObject suprRug;
    public GameObject genesisPassport;
    public GameObject suprTableAndChair;
    public GameObject poolTable;
    public GameObject suprPlant;
    public GameObject bed;
    public GameObject oldRug;

    // Reference to Buy Buttons
    public Button nftPosterButton;
    public Button roomExtensionButton;
    public Button suprRugButton;
    public Button genesisPassportButton;
    public Button suprTableAndChairButton;
    public Button poolTableButton;
    public Button suprPlantButton;

    // Player balance from GameManager
    private float balance;

    // Flags to track if items have been bought
    private bool isNFTPosterBought = false;
    private bool isRoomExtended = false;
    private bool isSuprRugBought = false;
    private bool isGenesisPassportBought = false;
    private bool isSuprTableAndChairBought = false;
    private bool isPoolTableBought = false;
    private bool isSuprPlantBought = false;

    // Item prices
    private float nftPosterPrice = 200f;
    private float roomExtensionPrice = 1000f;
    private float suprRugPrice = 1000f;
    private float genesisPassportPrice = 2000f;
    private float suprTableAndChairPrice = 5000f;
    private float poolTablePrice = 10000f;
    private float suprPlantPrice = 20000f;

    void Start()
    {
        // Initialize balance from GameManager
        balance = GameManager.suprBalance;

        // Set all objects inactive at start
        if (nftPoster != null) nftPoster.SetActive(false);
        if (extendedRoom != null) extendedRoom.SetActive(false);
        if (suprRug != null) suprRug.SetActive(false);
        if (genesisPassport != null) genesisPassport.SetActive(false);
        if (suprTableAndChair != null) suprTableAndChair.SetActive(false);
        if (poolTable != null) poolTable.SetActive(false);
        if (suprPlant != null) suprPlant.SetActive(false);
    }

    // Buy NFT Poster (first item, no prerequisite)
    public void BuyNFTPoster()
    {
        if (!isNFTPosterBought && nftPoster != null && balance >= nftPosterPrice)
        {
            nftPoster.SetActive(true);
            isNFTPosterBought = true;
            balance -= nftPosterPrice;
            GameManager.suprBalance = balance;
            UpdateButton(nftPosterButton);
            Debug.Log("NFT Poster purchased for " + nftPosterPrice + "! New balance: " + balance);
        }
        else if (isNFTPosterBought)
        {
            Debug.Log("NFT Poster already purchased!");
        }
        else
        {
            Debug.Log("Not enough balance! Need " + nftPosterPrice + ", have " + balance);
        }
    }

    // Buy Room Extension (requires NFT Poster)
    public void BuyRoomExtension()
    {
        if (!isRoomExtended && extendedRoom != null && balance >= roomExtensionPrice && isNFTPosterBought)
        {
            extendedRoom.SetActive(true);
            if (bed != null)
            {
                bed.SetActive(false);
            }
            if (oldRug != null)
            {
                oldRug.SetActive(false);
            }
            isRoomExtended = true;
            balance -= roomExtensionPrice;
            GameManager.suprBalance = balance;
            UpdateButton(roomExtensionButton);
            Debug.Log("Room Extension purchased for " + roomExtensionPrice + "! New balance: " + balance);
        }
        else if (isRoomExtended)
        {
            Debug.Log("Room Extension already purchased!");
        }
        else if (!isNFTPosterBought)
        {
            Debug.Log("Must purchase NFT Poster first!");
        }
        else
        {
            Debug.Log("Not enough balance! Need " + roomExtensionPrice + ", have " + balance);
        }
    }

    // Buy Supr Rug (requires Room Extension)
    public void BuySuprRug()
    {
        if (!isSuprRugBought && suprRug != null && balance >= suprRugPrice && isRoomExtended)
        {
            suprRug.SetActive(true);
            isSuprRugBought = true;
            balance -= suprRugPrice;
            GameManager.suprBalance = balance;
            UpdateButton(suprRugButton);
            Debug.Log("Supr Rug purchased for " + suprRugPrice + "! New balance: " + balance);
        }
        else if (isSuprRugBought)
        {
            Debug.Log("Supr Rug already purchased!");
        }
        else if (!isRoomExtended)
        {
            Debug.Log("Must purchase Room Extension first!");
        }
        else
        {
            Debug.Log("Not enough balance! Need " + suprRugPrice + ", have " + balance);
        }
    }

    // Buy Genesis Passport (requires Supr Rug)
    public void BuyGenesisPassport()
    {
        if (!isGenesisPassportBought && genesisPassport != null && balance >= genesisPassportPrice && isSuprRugBought)
        {
            genesisPassport.SetActive(true);
            isGenesisPassportBought = true;
            balance -= genesisPassportPrice;
            GameManager.suprBalance = balance;
            UpdateButton(genesisPassportButton);
            Debug.Log("Genesis Passport purchased for " + genesisPassportPrice + "! New balance: " + balance);
        }
        else if (isGenesisPassportBought)
        {
            Debug.Log("Genesis Passport already purchased!");
        }
        else if (!isSuprRugBought)
        {
            Debug.Log("Must purchase Supr Rug first!");
        }
        else
        {
            Debug.Log("Not enough balance! Need " + genesisPassportPrice + ", have " + balance);
        }
    }

    // Buy Supr Table and Chair (requires Genesis Passport)
    public void BuySuprTableAndChair()
    {
        if (!isSuprTableAndChairBought && suprTableAndChair != null && balance >= suprTableAndChairPrice && isGenesisPassportBought)
        {
            suprTableAndChair.SetActive(true);
            isSuprTableAndChairBought = true;
            balance -= suprTableAndChairPrice;
            GameManager.suprBalance = balance;
            UpdateButton(suprTableAndChairButton);
            Debug.Log("Supr Table and Chair purchased for " + suprTableAndChairPrice + "! New balance: " + balance);
        }
        else if (isSuprTableAndChairBought)
        {
            Debug.Log("Supr Table and Chair already purchased!");
        }
        else if (!isGenesisPassportBought)
        {
            Debug.Log("Must purchase Genesis Passport first!");
        }
        else
        {
            Debug.Log("Not enough balance! Need " + suprTableAndChairPrice + ", have " + balance);
        }
    }

    // Buy Pool Table (requires Supr Table and Chair)
    public void BuyPoolTable()
    {
        if (!isPoolTableBought && poolTable != null && balance >= poolTablePrice && isSuprTableAndChairBought)
        {
            poolTable.SetActive(true);
            isPoolTableBought = true;
            balance -= poolTablePrice;
            GameManager.suprBalance = balance;
            UpdateButton(poolTableButton);
            Debug.Log("Pool Table purchased for " + poolTablePrice + "! New balance: " + balance);
        }
        else if (isPoolTableBought)
        {
            Debug.Log("Pool Table already purchased!");
        }
        else if (!isSuprTableAndChairBought)
        {
            Debug.Log("Must purchase Supr Table and Chair first!");
        }
        else
        {
            Debug.Log("Not enough balance! Need " + poolTablePrice + ", have " + balance);
        }
    }

    // Buy Supr Plant (requires Pool Table)
    public void BuySuprPlant()
    {
        if (!isSuprPlantBought && suprPlant != null && balance >= suprPlantPrice && isPoolTableBought)
        {
            suprPlant.SetActive(true);
            isSuprPlantBought = true;
            balance -= suprPlantPrice;
            GameManager.suprBalance = balance;
            UpdateButton(suprPlantButton);
            Debug.Log("Supr Plant purchased for " + suprPlantPrice + "! New balance: " + balance);
        }
        else if (isSuprPlantBought)
        {
            Debug.Log("Supr Plant already purchased!");
        }
        else if (!isPoolTableBought)
        {
            Debug.Log("Must purchase Pool Table first!");
        }
        else
        {
            Debug.Log("Not enough balance! Need " + suprPlantPrice + ", have " + balance);
        }
    }

    // Helper method to update button state
    private void UpdateButton(Button button)
    {
        if (button != null)
        {
            // Get the TextMeshProUGUI component from the button's children
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "Bought";
                buttonText.color = Color.red;
            }
            button.interactable = false; // Disable the button
        }
    }
}