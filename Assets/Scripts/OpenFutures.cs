using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFutures : MonoBehaviour
{
    public GameObject room;
    public GameObject futuresCanvas;
    public GameObject carShopCanvas;
    
    // Start is called before the first frame update
    public void OnMouseDown()
    {
        
        room.SetActive(false);
        futuresCanvas.SetActive(true);
    }
}
