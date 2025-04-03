using UnityEngine;

public class SpecialObjectCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Assuming the player has a tag "Player"
        if (other.CompareTag("Player"))
        {
            GameManager.collectedCoins += 1;
            PlatformGenerator.specialObjectCounter++; // Increase counter
            Debug.Log("Special Objects Collected: " + PlatformGenerator.specialObjectCounter);
            Destroy(gameObject); // Destroy the special object
        }
    }
}