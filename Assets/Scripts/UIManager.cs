using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] // Add SerializeField attribute to ensure Unity serializes this field
    public GameObject jumpGamePrefab;
    private GameObject activeJumpGame;

    public GameObject room;
    public GameObject computerScreen;
    public GameObject futuresCanvas;
    public GameObject carsShopCanvas;
    public GameObject bidGameCanvas;
    public GameObject garage;

    public Button takeLoanButton;
    public Button runAdButton;
    public Button hireEmployeeButton;
    
    //public GameObject upgradePanel;
    public TMP_InputField loanAmountInput; // Assign in Inspector

    void Awake()
    {
        // Add validation in Awake
        if (jumpGamePrefab == null)
        {
            Debug.LogError("jumpGamePrefab is not assigned in the inspector! Please assign it.");
        }
        else
        {
            Debug.Log("jumpGamePrefab is properly assigned: " + jumpGamePrefab.name);
        }
    }

    void Start()
    {
        // Add another check in Start
        Debug.Log("Start - jumpGamePrefab status: " + (jumpGamePrefab != null ? jumpGamePrefab.name : "null"));

        takeLoanButton.onClick.AddListener(() =>
        {
            if (float.TryParse(loanAmountInput.text, out float amount))
            {
                GameManager.Instance.TakeLoan(amount);
            }
        });

        runAdButton.onClick.AddListener(() =>
        {
            // Add debug log
            Debug.Log("Run Ad button clicked. jumpGamePrefab status: " + (jumpGamePrefab != null ? jumpGamePrefab.name : "null"));
            
            if (activeJumpGame != null)
            {
                DestroyJumpGame();
            }
            OpenJumpGame();
        });

        hireEmployeeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.HireEmployee();
            // upgradePanel.SetActive(false);
        });
    }

    public void OpenShop()
    {
        DestroyJumpGame();
        garage.SetActive(false);
        bidGameCanvas.SetActive(false);
        room.SetActive(false);
        computerScreen.SetActive(false);
        futuresCanvas.SetActive(false);
        carsShopCanvas.SetActive(true);
    }

    public void OpenComputer()
    {
        DestroyJumpGame();
        garage.SetActive(false);
        bidGameCanvas.SetActive(false);
        room.SetActive(false);
        computerScreen.SetActive(true);
        futuresCanvas.SetActive(false);
        carsShopCanvas.SetActive(false);
    }

    public void OpenJumpGame()
    {
        // Add validation and debug logs
        if (jumpGamePrefab == null)
        {
            Debug.LogError("Cannot open jump game - jumpGamePrefab is null!");
            return;
        }

        Debug.Log("Attempting to instantiate jumpGamePrefab: " + jumpGamePrefab.name);
        
        if (activeJumpGame == null)
        {
            activeJumpGame = Instantiate(jumpGamePrefab);
            Debug.Log("Jump game instantiated successfully: " + (activeJumpGame != null));
            activeJumpGame.SetActive(true);
        } else if (activeJumpGame != null)
        {
            DestroyJumpGame();
            activeJumpGame = Instantiate(jumpGamePrefab);
            Debug.Log("Jump game instantiated successfully: " + (activeJumpGame != null));
            activeJumpGame.SetActive(true);
        }
        
        garage.SetActive(false);
        bidGameCanvas.SetActive(false);
        room.SetActive(false);
        computerScreen.SetActive(false);
        futuresCanvas.SetActive(false);
        carsShopCanvas.SetActive(false);
    }

    private void DestroyJumpGame()
    {
        if (activeJumpGame != null)
        {
            Debug.Log("Destroying active jump game instance");
            Destroy(activeJumpGame);
            activeJumpGame = null;
        }
    }

    public void OpenFutures()
    {
        DestroyJumpGame();
        garage.SetActive(false);
        bidGameCanvas.SetActive(false);
        room.SetActive(false);
        computerScreen.SetActive(false);
        futuresCanvas.SetActive(true);
        carsShopCanvas.SetActive(false);
    }

    public void OpenBidGame()
    {
        DestroyJumpGame();
        garage.SetActive(false);
        bidGameCanvas.SetActive(true);
        room.SetActive(false);
        computerScreen.SetActive(false);
        futuresCanvas.SetActive(false);
        carsShopCanvas.SetActive(false);
    }

    public void OpenGarage()
    {
        DestroyJumpGame();
        garage.SetActive(true);
        bidGameCanvas.SetActive(false);
        room.SetActive(false);
        computerScreen.SetActive(false);
        futuresCanvas.SetActive(false);
        carsShopCanvas.SetActive(false);
    }
    
    public void ReturnToRoom()
    {
        DestroyJumpGame();
        garage.SetActive(false);
        bidGameCanvas.SetActive(false);
        room.SetActive(true);
        computerScreen.SetActive(false);
        futuresCanvas.SetActive(false);
        carsShopCanvas.SetActive(false);
    }
}