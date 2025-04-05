using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public GameObject jumpGamePrefab;
    private GameObject activeJumpGame;

    public GameObject room;
    public GameObject computerScreen;
    public GameObject futuresCanvas;
    public GameObject carsShopCanvas;
    public GameObject bidGameCanvas;
    public GameObject garage;
    public GameObject shop;
    public GameObject menuPanel;

    public Button takeLoanButton;
    public Button runAdButton;
    public Button hireEmployeeButton;
    public Button maxLoanButton; // New button reference

    public TMP_InputField loanAmountInput; // Already assigned in Inspector

    void Awake()
    {
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
        Debug.Log("Start - jumpGamePrefab status: " + (jumpGamePrefab != null ? jumpGamePrefab.name : "null"));

        // Existing button listeners
        takeLoanButton.onClick.AddListener(() =>
        {
            if (float.TryParse(loanAmountInput.text, out float amount))
            {
                GameManager.Instance.TakeLoan(amount);
            }
        });

        runAdButton.onClick.AddListener(() =>
        {
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
        });

        // New Max Loan button listener
        if (maxLoanButton != null)
        {
            maxLoanButton.onClick.AddListener(() =>
            {
                loanAmountInput.text = GameManager.Instance.maxAmountToLoan.ToString("F2");
                Debug.Log($"Max loan amount set in input field: {GameManager.Instance.maxAmountToLoan:F2}");
            });
        }
        else
        {
            Debug.LogError("maxLoanButton is not assigned in the Inspector!");
        }
    }

    // Existing methods (OpenShop, OpenComputer, etc.) remain unchanged
    public void OpenShop()
    {
        menuPanel.SetActive(false);
        
        shop.SetActive(false);
        DestroyJumpGame();
        garage.SetActive(false);
        bidGameCanvas.SetActive(false);
        room.SetActive(false);
        computerScreen.SetActive(false);
        futuresCanvas.SetActive(false);
        carsShopCanvas.SetActive(true);
    }
    
    public void OpenAssetsShop()
    {
        menuPanel.SetActive(false);
        
        shop.SetActive(true);
        DestroyJumpGame();
        garage.SetActive(false);
        bidGameCanvas.SetActive(false);
        room.SetActive(false);
        computerScreen.SetActive(false);
        futuresCanvas.SetActive(false);
        carsShopCanvas.SetActive(false);
    }

    public void OpenComputer()
    {
        shop.SetActive(false);
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
        }
        else
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
        shop.SetActive(false);
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
        shop.SetActive(false);
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
        shop.SetActive(false);
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
        shop.SetActive(false);
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
        shop.SetActive(false);
        DestroyJumpGame();
        garage.SetActive(false);
        bidGameCanvas.SetActive(false);
        room.SetActive(true);
        computerScreen.SetActive(false);
        futuresCanvas.SetActive(false);
        carsShopCanvas.SetActive(false);
    }
}