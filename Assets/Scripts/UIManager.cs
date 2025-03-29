using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
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

    void Start()
    {
        takeLoanButton.onClick.AddListener(() =>
        {
            if (float.TryParse(loanAmountInput.text, out float amount))
            {
                GameManager.Instance.TakeLoan(amount);
            }
        });

        runAdButton.onClick.AddListener(() =>
        {
            GameManager.Instance.RunAdCampaign();
        });

        hireEmployeeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.HireEmployee();
            // upgradePanel.SetActive(false);
        });
    }

    public void OpenShop()
    {
        garage.SetActive(false);
        bidGameCanvas.SetActive(false);
        room.SetActive(false);
        computerScreen.SetActive(false);
        futuresCanvas.SetActive(false);
        carsShopCanvas.SetActive(true);
    }

    public void OpenFutures()
    {
        garage.SetActive(false);
        bidGameCanvas.SetActive(false);
        room.SetActive(false);
        computerScreen.SetActive(false);
        futuresCanvas.SetActive(true);
        carsShopCanvas.SetActive(false);
    }

    public void OpenBidGame()
    {
        garage.SetActive(false);
        bidGameCanvas.SetActive(true);
        room.SetActive(false);
        computerScreen.SetActive(false);
        futuresCanvas.SetActive(false);
        carsShopCanvas.SetActive(false);
    }

    public void OpenGarage()
    {
        garage.SetActive(true);
        bidGameCanvas.SetActive(false);
        room.SetActive(false);
        computerScreen.SetActive(false);
        futuresCanvas.SetActive(false);
        carsShopCanvas.SetActive(false);
    }
    
    public void ReturnToRoom()
    {
        garage.SetActive(false);
        bidGameCanvas.SetActive(false);
        room.SetActive(true);
        computerScreen.SetActive(false);
        futuresCanvas.SetActive(false);
        carsShopCanvas.SetActive(false);
    }

    // public void ToggleUpgradePanel()
    // {
    //     upgradePanel.SetActive(!upgradePanel.activeSelf);
    // }
}