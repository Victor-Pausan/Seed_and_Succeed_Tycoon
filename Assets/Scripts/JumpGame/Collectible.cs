using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float rotateSpeed = 100f;
    public float bobAmplitude = 0.5f;
    public float bobFrequency = 1f;
    
    private Vector3 startPos;
    
    void Start()
    {
        startPos = transform.position;
    }
    
    void Update()
    {
        // Rotate the collectible
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        
        // Bob up and down
        float newY = startPos.y + Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}