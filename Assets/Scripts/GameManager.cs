using System;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Net.Mime;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static float suprBalance = 1000000f;
    public float loanBalance = 0f;
    public float loanCollateralRatio = 5f;
    public float adEffectiveness = 1f;
    public int employees = 0;
    public float adCost = 10f; // Starting ad cost
    public float collateralBalance = 0f;
<<<<<<< aba88094fa5ef074156755298690050001224834
    private float repaymentAmount = 0f; // Repayment rate, starts with a base value
=======
    public static float repaymentAmount = 0f; // Total repayment rate (base + campaign boosts)
    private float baseRepaymentRate = 0.05f; // Passive repayment rate, increased by employees
>>>>>>> finished jumping game
    private int adCampaigns = 0;
    private bool eligibleToBuyAd = false;
<<<<<<< aba88094fa5ef074156755298690050001224834
=======
    private float baseHireCost = 30f;
    public float maxAmountToLoan = 0 ;
<<<<<<< aba88094fa5ef074156755298690050001224834
>>>>>>> modified UI
=======
    public static float collectedCoins = 0;
>>>>>>> finished jumping game

    public TextMeshProUGUI suprText;
    public TextMeshProUGUI loanText;
    public TextMeshProUGUI collatertalText;
    public TextMeshProUGUI runAdText;
<<<<<<< aba88094fa5ef074156755298690050001224834
=======
    public TextMeshProUGUI employeesText; // New UI to display employee count
    public TextMeshProUGUI maxAmountLoan;
    public TextMeshProUGUI repaymentRatio;
>>>>>>> modified UI

    void Awake()
    {
        // Singleton pattern with DontDestroyOnLoad
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Set a base repayment rate and start the repayment system
        repaymentAmount = 0.05f; // Small base rate (adjust as needed)
        Debug.Log($"Repayment system started with base rate: {repaymentAmount}");
        StartCoroutine(RepayLoanCoroutine());
    }

    void Update()
    {
<<<<<<< aba88094fa5ef074156755298690050001224834
        // Only update UI if references exist
        if (suprText != null) suprText.text = $"SUPR: {suprBalance:F2}";
        if (loanText != null) loanText.text = $"Loan: {loanBalance:F2} SUPR";
        if (collatertalText != null) collatertalText.text = $"Collateral: {collateralBalance:F2} SUPR";
        if (runAdText != null) runAdText.text = $"Run ad campaign: {adCost:F2} SUPR";
=======
        maxAmountToLoan = suprBalance / loanCollateralRatio;
        // Update UI if references exist
        if (suprText != null) suprText.text = $"SUPR balance: {suprBalance:F2} (loan up to {maxAmountToLoan})";
        if (loanText != null) loanText.text = $"Loaned: {loanBalance:F2} SUPR";
        if (collatertalText != null) collatertalText.text = $"Collateral SUPR: {collateralBalance:F2}";
        if (runAdText != null) runAdText.text = $"SUPR GAME";
        if (employeesText != null) employeesText.text = $"Hire employee: {baseHireCost} SUPR";
        if (maxAmountLoan != null) maxAmountLoan.text = $"Max amount to loan: {maxAmountToLoan}";
        if (repaymentRatio != null) repaymentRatio.text = $"Repayment ratio: {repaymentAmount} supr/s";
>>>>>>> modified UI
    }

    public void TakeLoan(float amount)
    {
        float requiredCollateral = amount * loanCollateralRatio;
        if (suprBalance >= requiredCollateral)
        {
            suprBalance -= requiredCollateral;
            suprBalance += amount;
            collateralBalance = requiredCollateral;
            loanBalance += amount;

            eligibleToBuyAd = true;
            Debug.Log($"Loan taken: {amount} Supr");
        }
        else
        {
            Debug.Log("Not enough collateral!");
        }
    }

    public void RunAdCampaign()
    {
        if (suprBalance >= adCost && eligibleToBuyAd)
        {
            adCampaigns += 1;
            
            // Increase ad cost exponentially with each campaign
            adCost = 10f * Mathf.Pow(2, adCampaigns - 1);
            
            suprBalance -= adCost;
            float newUsers = adCost * adEffectiveness * 0.1f;
            float campaignRepayment = newUsers * 0.1f; // Repayment boost from this campaign
            repaymentAmount += campaignRepayment; // Add to the existing repayment rate
            Debug.Log($"Ad campaign {adCampaigns} ran! New users: {newUsers}, Added repayment: {campaignRepayment}, Total repayment rate: {repaymentAmount}");
        }
        else
        {
            Debug.Log("Cannot run ad campaign: insufficient funds or no loan taken!");
        }
    }

    private IEnumerator RepayLoanCoroutine()
    {
        while (true) // Runs constantly throughout the game
        {
            yield return new WaitForSeconds(0.1f); // Adjust interval (0.1s for testing, 10s for slower pace)

            if (loanBalance > 0 && repaymentAmount > 0)
            {
                float repaymentThisTick = repaymentAmount; // Amount to repay this tick
                loanBalance = Mathf.Max(0, loanBalance - repaymentThisTick);
                suprBalance += repaymentThisTick * 5; // Return collateral as loan is repaid
                collateralBalance = Mathf.Max(0, collateralBalance - repaymentThisTick * 5); // Reduce collateral
                Debug.Log($"Loan repaid by {repaymentThisTick}. Remaining loan: {loanBalance}, Collateral: {collateralBalance}");
            }
            else if (loanBalance <= 0 && collateralBalance > 0)
            {
                // If loan is fully repaid but collateral remains, return it fully
                suprBalance += collateralBalance;
                Debug.Log($"Loan fully repaid! Collateral returned: {collateralBalance}");
                collateralBalance = 0;
            }
        }
    }

    public void HireEmployee()
    {
<<<<<<< aba88094fa5ef074156755298690050001224834
        float cost = 100f;
        if (suprBalance >= cost)
=======
        float hireCost = baseHireCost * Mathf.Pow(1.5f, employees); // Cost increases with each hire

        if (suprBalance >= hireCost && eligibleToBuyAd)
        {
            suprBalance -= hireCost;
            employees += 1;

            // Employee benefits
            adEffectiveness += 0.5f; // Increase ad effectiveness
            baseRepaymentRate += 0.02f; // Increase base repayment rate
            repaymentAmount = baseRepaymentRate + (adCampaigns > 0 ? repaymentAmount - baseRepaymentRate : 0); // Update total repayment rate

            Debug.Log($"Employee hired! Total: {employees}, Cost: {hireCost:F2}, New ad effectiveness: {adEffectiveness}, New base repayment rate: {baseRepaymentRate}");
        }
        else
>>>>>>> modified UI
        {
            suprBalance -= cost;
            employees++;
            adEffectiveness += 0.5f;
            Debug.Log($"Employee hired! Total: {employees}");
        }
    }

    // Optional: For updating UI references across scenes
    public void UpdateUIReferences(TextMeshProUGUI newSuprText, TextMeshProUGUI newLoanText, 
                                   TextMeshProUGUI newCollateralText, TextMeshProUGUI newRunAdText)
    {
        suprText = newSuprText;
        loanText = newLoanText;
        collatertalText = newCollateralText;
        runAdText = newRunAdText;
    }
}