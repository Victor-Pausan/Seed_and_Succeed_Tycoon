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
        // Set all objects inactive at start
        if (nftPoster != null) nftPoster.SetActive(false);
        if (extendedRoom != null) extendedRoom.SetActive(false);
        if (suprRug != null) suprRug.SetActive(false);
        if (genesisPassport != null) genesisPassport.SetActive(false);
        if (suprTableAndChair != null) suprTableAndChair.SetActive(false);
        if (poolTable != null) poolTable.SetActive(false);
        if (suprPlant != null) suprPlant.SetActive(false);
    }

    public void BuyNFTPoster()
    {
        if (!isNFTPosterBought && nftPoster != null && GameManager.suprBalance >= nftPosterPrice)
        {
            nftPoster.SetActive(true);
            isNFTPosterBought = true;
            GameManager.suprBalance -= nftPosterPrice;
            UpdateButton(nftPosterButton);
            Debug.Log("NFT Poster purchased for " + nftPosterPrice + "! New balance: " + GameManager.suprBalance);
        }
        else if (isNFTPosterBought)
        {
            Debug.Log("NFT Poster already purchased!");
        }
        else
        {
            Debug.Log("Not enough balance! Need " + nftPosterPrice + ", have " + GameManager.suprBalance);
        }
    }

    public void BuyRoomExtension()
    {
        if (!isRoomExtended && extendedRoom != null && GameManager.suprBalance >= roomExtensionPrice)
        {
            extendedRoom.SetActive(true);
            if (bed != null) bed.SetActive(false);
            if (oldRug != null) oldRug.SetActive(false);
            isRoomExtended = true;
            GameManager.suprBalance -= roomExtensionPrice;
            UpdateButton(roomExtensionButton);
            Debug.Log("Room Extension purchased for " + roomExtensionPrice + "! New balance: " + GameManager.suprBalance);
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
            Debug.Log("Not enough balance! Need " + roomExtensionPrice + ", have " + GameManager.suprBalance);
        }
    }

    public void BuySuprRug()
    {
        if (!isSuprRugBought && suprRug != null && GameManager.suprBalance >= suprRugPrice)
        {
            suprRug.SetActive(true);
            isSuprRugBought = true;
            GameManager.suprBalance -= suprRugPrice;
            UpdateButton(suprRugButton);
            Debug.Log("Supr Rug purchased for " + suprRugPrice + "! New balance: " + GameManager.suprBalance);
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
            Debug.Log("Not enough balance! Need " + suprRugPrice + ", have " + GameManager.suprBalance);
        }
    }

    public void BuyGenesisPassport()
    {
        if (!isGenesisPassportBought && genesisPassport != null && GameManager.suprBalance >= genesisPassportPrice)
        {
            genesisPassport.SetActive(true);
            isGenesisPassportBought = true;
            GameManager.suprBalance -= genesisPassportPrice;
            UpdateButton(genesisPassportButton);
            Debug.Log("Genesis Passport purchased for " + genesisPassportPrice + "! New balance: " + GameManager.suprBalance);
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
            Debug.Log("Not enough balance! Need " + genesisPassportPrice + ", have " + GameManager.suprBalance);
        }
    }

    public void BuySuprTableAndChair()
    {
        if (!isSuprTableAndChairBought && suprTableAndChair != null && GameManager.suprBalance >= suprTableAndChairPrice)
        {
            suprTableAndChair.SetActive(true);
            isSuprTableAndChairBought = true;
            GameManager.suprBalance -= suprTableAndChairPrice;
            UpdateButton(suprTableAndChairButton);
            Debug.Log("Supr Table and Chair purchased for " + suprTableAndChairPrice + "! New balance: " + GameManager.suprBalance);
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
            Debug.Log("Not enough balance! Need " + suprTableAndChairPrice + ", have " + GameManager.suprBalance);
        }
    }

    public void BuyPoolTable()
    {
        if (!isPoolTableBought && poolTable != null && GameManager.suprBalance >= poolTablePrice)
        {
            poolTable.SetActive(true);
            isPoolTableBought = true;
            GameManager.suprBalance -= poolTablePrice;
            UpdateButton(poolTableButton);
            Debug.Log("Pool Table purchased for " + poolTablePrice + "! New balance: " + GameManager.suprBalance);
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
            Debug.Log("Not enough balance! Need " + poolTablePrice + ", have " + GameManager.suprBalance);
        }
    }

    public void BuySuprPlant()
    {
        if (!isSuprPlantBought && suprPlant != null && GameManager.suprBalance >= suprPlantPrice)
        {
            suprPlant.SetActive(true);
            isSuprPlantBought = true;
            GameManager.suprBalance -= suprPlantPrice;
            UpdateButton(suprPlantButton);
            Debug.Log("Supr Plant purchased for " + suprPlantPrice + "! New balance: " + GameManager.suprBalance);
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
            Debug.Log("Not enough balance! Need " + suprPlantPrice + ", have " + GameManager.suprBalance);
        }
    }

    private void UpdateButton(Button button)
    {
        if (button != null)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "Bought";
                buttonText.color = Color.red;
            }
            button.interactable = false;
        }
    }
}
