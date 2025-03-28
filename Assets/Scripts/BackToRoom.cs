using UnityEngine;

public class BackToRoom : MonoBehaviour
{
    public GameObject dashboardCanvas;
    public GameObject room;
    public GameObject futuresCanvas;
    public void OnMouseDown()
    {
        room.SetActive(true);
        dashboardCanvas.SetActive(false);
        futuresCanvas.SetActive(false);
    }
}