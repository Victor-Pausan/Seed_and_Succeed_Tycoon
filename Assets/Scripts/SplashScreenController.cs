using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    public Button playButton;     // Assign in Inspector
    public GameObject spinner;    // Assign in Inspector

    void Start()
    {
        // Ensure button is enabled and spinner is hidden at start
        if (playButton != null)
        {
            playButton.onClick.AddListener(StartLoading);
            playButton.interactable = true; // Button starts enabled
        }
        else
        {
            Debug.LogError("PlayButton not assigned in SplashScreenController!");
        }

        if (spinner != null)
        {
            spinner.SetActive(false); // Spinner starts hidden
        }
        else
        {
            Debug.LogError("Spinner not assigned in SplashScreenController!");
        }
    }

    void StartLoading()
    {
        // Disable the button and show the spinner
        playButton.interactable = false;
        spinner.SetActive(true);

        // Start loading the game scene asynchronously
        StartCoroutine(LoadGameSceneAsync());
    }

    IEnumerator LoadGameSceneAsync()
    {
        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1); // Index 1 is your game scene
        asyncLoad.allowSceneActivation = true; // Allow scene to activate when fully loaded

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            // Optional: Log progress for debugging
            Debug.Log($"Loading progress: {asyncLoad.progress * 100}%");
            yield return null;
        }
    }
}