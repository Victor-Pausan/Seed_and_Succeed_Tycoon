using UnityEngine;
using UnityEngine.UI;

public class WelcomeScreenController : MonoBehaviour
{
    public GameObject welcomePanel; // Assign the WelcomePanel in Inspector
    public Button continueButton;   // Assign the ContinueButton in Inspector

    void Start()
    {
        // Ensure the welcome screen is visible at start
        if (welcomePanel != null)
        {
            welcomePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("WelcomePanel not assigned in WelcomeScreenController!");
        }

        // Set up the continue button
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(HideWelcomeScreen);
        }
        else
        {
            Debug.LogError("ContinueButton not assigned in WelcomeScreenController!");
        }
    }

    void HideWelcomeScreen()
    {
        welcomePanel.SetActive(false); // Hide the welcome screen
    }
}
