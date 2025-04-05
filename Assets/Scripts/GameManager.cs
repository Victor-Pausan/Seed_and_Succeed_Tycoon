using System;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Net.Mime;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static float suprBalance = 1000f;
    public float loanBalance = 0f;
    public float loanCollateralRatio = 5f;
    public float adEffectiveness = 1f; // Base ad effectiveness, boosted by employees
    public int employees = 0;
    public float adCost = 10f; // Starting ad cost
    public float collateralBalance = 0f;
    public static float repaymentAmount = 0f; // Total repayment rate (base + campaign boosts)
    private float baseRepaymentRate = 0.05f; // Passive repayment rate, increased by employees
    private int adCampaigns = 0;
    private bool eligibleToBuyAd = false;
    private float baseHireCost = 30f;
    public float maxAmountToLoan = 0 ;
    public static float collectedCoins = 0;

    public TextMeshProUGUI suprText;
    public TextMeshProUGUI loanText;
    public TextMeshProUGUI collatertalText;
    public TextMeshProUGUI runAdText;
    public TextMeshProUGUI employeesText; // New UI to display employee count
    public TextMeshProUGUI maxAmountLoan;
    public TextMeshProUGUI repaymentRatio;

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
        // Initialize repayment with base rate
        repaymentAmount = baseRepaymentRate;
        Debug.Log($"Repayment system started with base rate: {repaymentAmount}");
        StartCoroutine(RepayLoanCoroutine());
    }

    void Update()
    {
        maxAmountToLoan = suprBalance / loanCollateralRatio;
        // Update UI if references exist
        if (suprText != null) suprText.text = $"SUPR balance: {suprBalance:F2} (loan up to {maxAmountToLoan})";
        if (loanText != null) loanText.text = $"Loaned: {loanBalance:F2} SUPR";
        if (collatertalText != null) collatertalText.text = $"Collateral SUPR: {collateralBalance:F2}";
        if (runAdText != null) runAdText.text = $"SUPR GAME";
        if (employeesText != null) employeesText.text = $"Hire employee: {baseHireCost} SUPR";
        if (maxAmountLoan != null) maxAmountLoan.text = $"Max amount to loan: {maxAmountToLoan}";
        if (repaymentRatio != null) repaymentRatio.text = $"Repayment ratio: {repaymentAmount} supr/s";
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
            repaymentAmount += campaignRepayment; // Add to total repayment rate
            Debug.Log($"Ad campaign {adCampaigns} ran! New users: {newUsers}, Added repayment: {campaignRepayment}, Total repayment rate: {repaymentAmount}");
        }
        else
        {
            Debug.Log("Cannot run ad campaign: insufficient funds or no loan taken!");
        }
    }

    private IEnumerator RepayLoanCoroutine()
    {
        while (true) // Runs constantly
        {
            yield return new WaitForSeconds(0.1f); // 0.1s for testing, adjust to 10s if desired

            if (loanBalance > 0 && repaymentAmount > 0)
            {
                float repaymentThisTick = repaymentAmount;
                loanBalance = Mathf.Max(0, loanBalance - repaymentThisTick);
                suprBalance += repaymentThisTick * 5; // Return collateral
                collateralBalance = Mathf.Max(0, collateralBalance - repaymentThisTick * 5);
                Debug.Log($"Loan repaid by {repaymentThisTick}. Remaining loan: {loanBalance}, Collateral: {collateralBalance}");
            }
            else if (loanBalance <= 0 && collateralBalance > 0)
            {
                suprBalance += collateralBalance;
                Debug.Log($"Loan fully repaid! Collateral returned: {collateralBalance}");
                collateralBalance = 0;
            }
        }
    }

    public void HireEmployee()
    {
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
        {
            Debug.Log($"Not enough Supr to hire employee! Required: {hireCost:F2}");
        }
    }

    // For updating UI references across scenes
    public void UpdateUIReferences(TextMeshProUGUI newSuprText, TextMeshProUGUI newLoanText,
                                   TextMeshProUGUI newCollateralText, TextMeshProUGUI newRunAdText,
                                   TextMeshProUGUI newEmployeesText)
    {
        suprText = newSuprText;
        loanText = newLoanText;
        collatertalText = newCollateralText;
        runAdText = newRunAdText;
        employeesText = newEmployeesText;
    }
}