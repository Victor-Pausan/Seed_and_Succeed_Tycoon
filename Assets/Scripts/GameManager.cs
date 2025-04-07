using System;
using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static float suprBalance = 1000f;
    public float loanBalance = 0f;
    public float loanCollateralRatio = 5f;
    public float adEffectiveness = 1f;
    public int employees = 0;
    public float adCost = 10f;
    public float collateralBalance = 0f;
    public static float repaymentAmount = 0f;
    private float baseRepaymentRate = 0.05f;
    private int adCampaigns = 0;
    private bool eligibleToBuyAd = false;
    private float baseHireCost = 30f;
    public float maxAmountToLoan = 0;
    public static float collectedCoins = 0;

    public TextMeshProUGUI suprText;
    public TextMeshProUGUI loanText;
    public TextMeshProUGUI collatertalText;
    public TextMeshProUGUI runAdText;
    public TextMeshProUGUI employeesText;
    public TextMeshProUGUI maxAmountLoan;
    public TextMeshProUGUI repaymentRatio;

    void Awake()
    {
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
        repaymentAmount = baseRepaymentRate;
        repaymentAmount = RoundDownToTwoDecimals(repaymentAmount); // Initial rounding
        Debug.Log($"Repayment system started with base rate: {repaymentAmount}");
        StartCoroutine(RepayLoanCoroutine());
    }

    void Update()
    {
        maxAmountToLoan = RoundDownToTwoDecimals(suprBalance / loanCollateralRatio);
        if (suprText != null) suprText.text = $"SUPR balance: {suprBalance:F2} (loan up to {maxAmountToLoan:F2})";
        if (loanText != null) loanText.text = $"Loaned: {loanBalance:F2} SUPR";
        if (collatertalText != null) collatertalText.text = $"Collateral SUPR: {collateralBalance:F2}";
        if (runAdText != null) runAdText.text = $"SUPR GAME";
        if (employeesText != null) employeesText.text = $"Hire employee: {baseHireCost} SUPR";
        if (maxAmountLoan != null) maxAmountLoan.text = $"Max amount to loan: {maxAmountToLoan:F2}";
        if (repaymentRatio != null) repaymentRatio.text = $"Repayment ratio: {repaymentAmount * 10f:F2} supr/s";
    }

    public void TakeLoan(float amount)
    {
        float requiredCollateral = amount * loanCollateralRatio;
        if (suprBalance >= requiredCollateral)
        {
            suprBalance -= requiredCollateral;
            suprBalance += amount;
            suprBalance = RoundDownToTwoDecimals(suprBalance);
            collateralBalance += requiredCollateral;
            collateralBalance = RoundDownToTwoDecimals(collateralBalance);
            loanBalance += amount;
            loanBalance = RoundDownToTwoDecimals(loanBalance);

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
            adCost = 10f * Mathf.Pow(2, adCampaigns - 1);

            suprBalance -= adCost;
            suprBalance = RoundDownToTwoDecimals(suprBalance);
            float newUsers = adCost * adEffectiveness * 0.1f;
            float campaignRepayment = newUsers * 0.1f;
            repaymentAmount += campaignRepayment;
            repaymentAmount = RoundDownToTwoDecimals(repaymentAmount);
            Debug.Log($"Ad campaign {adCampaigns} ran! New users: {newUsers}, Added repayment: {campaignRepayment}, Total repayment rate: {repaymentAmount}");
        }
        else
        {
            Debug.Log("Cannot run ad campaign: insufficient funds or no loan taken!");
        }
    }

    private IEnumerator RepayLoanCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (loanBalance > 0 && repaymentAmount > 0)
            {
                float repaymentThisTick = repaymentAmount;
                loanBalance = Mathf.Max(0, loanBalance - repaymentThisTick);
                loanBalance = RoundDownToTwoDecimals(loanBalance);
                suprBalance += repaymentThisTick * 5;
                suprBalance = RoundDownToTwoDecimals(suprBalance);
                collateralBalance = Mathf.Max(0, collateralBalance - repaymentThisTick * 5);
                collateralBalance = RoundDownToTwoDecimals(collateralBalance);
                Debug.Log($"Loan repaid by {repaymentThisTick}. Remaining loan: {loanBalance}, Collateral: {collateralBalance}");
            }
            else if (loanBalance <= 0 && collateralBalance > 0)
            {
                suprBalance += collateralBalance;
                suprBalance = RoundDownToTwoDecimals(suprBalance);
                Debug.Log($"Loan fully repaid! Collateral returned: {collateralBalance}");
                collateralBalance = 0;
                collateralBalance = RoundDownToTwoDecimals(collateralBalance);
            }
        }
    }

    public void HireEmployee()
    {
        float hireCost = baseHireCost * Mathf.Pow(1.5f, employees);

        if (suprBalance >= hireCost && eligibleToBuyAd)
        {
            suprBalance -= hireCost;
            suprBalance = RoundDownToTwoDecimals(suprBalance);
            employees += 1;

            adEffectiveness += 0.5f;
            baseRepaymentRate += 0.02f;
            repaymentAmount = baseRepaymentRate + (adCampaigns > 0 ? repaymentAmount - baseRepaymentRate : 0);
            repaymentAmount = RoundDownToTwoDecimals(repaymentAmount);

            Debug.Log($"Employee hired! Total: {employees}, Cost: {hireCost:F2}, New ad effectiveness: {adEffectiveness}, New base repayment rate: {baseRepaymentRate}");
        }
        else
        {
            Debug.Log($"Not enough Supr to hire employee! Required: {hireCost:F2}");
        }
    }

    // Utility method to round down to 2 decimal places
    private float RoundDownToTwoDecimals(float value)
    {
        return Mathf.Floor(value * 100f) / 100f;
    }

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