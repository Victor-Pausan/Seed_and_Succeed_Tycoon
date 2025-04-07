using UnityEngine;
using UnityEngine.UI;
using TMPro; // If using TextMeshPro for button text

public class AudioToggle : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // The audio source to toggle
    [SerializeField] private Button toggleButton;     // The button that toggles audio
    [SerializeField] private Sprite audioOnImage;
    [SerializeField] private Sprite audioOffImage; 
    

    private bool isAudioEnabled = true; // Tracks the current audio state

    void Start()
    {
        // Validate references
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not assigned in AudioToggle script!");
            return;
        }

        if (toggleButton == null)
        {
            Debug.LogError("ToggleButton not assigned in AudioToggle script!");
            return;
        }

        // Set initial button state
        toggleButton.onClick.AddListener(ToggleAudio);
        UpdateButtonDisplay();
    }

    void ToggleAudio()
    {
        isAudioEnabled = !isAudioEnabled; // Toggle the state
        audioSource.mute = !isAudioEnabled; // Mute/unmute the audio
        UpdateButtonDisplay();
        Debug.Log("Audio " + (isAudioEnabled ? "enabled" : "disabled"));
    }

    void UpdateButtonDisplay()
    {
        if (toggleButton != null)
        {
            toggleButton.image.sprite = isAudioEnabled ? audioOnImage : audioOffImage;
        }
    }
}