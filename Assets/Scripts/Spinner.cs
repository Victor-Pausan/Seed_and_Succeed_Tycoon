using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float rotationSpeed = 200f; // Degrees per second

    void Update()
    {
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime); // Rotate clockwise
    }
}