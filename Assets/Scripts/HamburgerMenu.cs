using System;
using UnityEngine;
using UnityEngine.UI;

public class HamburgerMenu : MonoBehaviour
{
    public GameObject menuPanel; // Assign in Inspector
    public GameObject room;
    private bool isOpen = false;

    void Start()
    {
        menuPanel.SetActive(false); // Hide menu initially
    }

    public void Disable()
    {
        menuPanel.SetActive(false);
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        menuPanel.SetActive(isOpen);
    }
}