using UnityEngine;

public class ComputerInteract : MonoBehaviour
{
    public GameObject dashboardCanvas;

    public GameObject room;

    public GameObject jumpGame;
    //public GameObject upgradePanel;

    void Start()
    {
        dashboardCanvas.SetActive(false);
        room.SetActive(true);
        //upgradePanel.SetActive(false);
    }

    void OnMouseDown()
    {
        jumpGame.SetActive(false);
        dashboardCanvas.SetActive(!dashboardCanvas.activeSelf);
        room.SetActive(false);
        //upgradePanel.SetActive(false);
    }
}