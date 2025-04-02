using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject collectiblePrefab;
    public GameObject specialObjectPrefab; // New prefab for special object
    public int initialPlatformCount = 8; // Reduced from 13 to 8
    public float minY = 1.0f; // Increased from 0.5f to 1.0f
    public float maxY = 4.0f; // Increased from 3.0f to 4.0f
    public float collectibleSpawnChance = 0.2f;
    public float specialObjectSpawnChance = 1.0f; // Kept at 1.0f for testing
    public static int specialObjectCounter = 0; // Counter for special objects collected
    
    private float screenWidth;
    private float screenHeight;
    private Camera mainCamera;
    private List<GameObject> activePlatforms = new List<GameObject>();
    private float highestPlatformY = 0f;
    
    void Start()
    {
        mainCamera = Camera.main;
        screenWidth = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 2;
        screenHeight = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y * 2;
        
        InitializePlatforms();
    }

    public void InitializePlatforms()
    {
        // Clear any existing platforms first
        CleanupPlatforms();
        
        // Reset highest platform Y
        highestPlatformY = 0f;
        
        // Create initial platforms
        for (int i = 0; i < initialPlatformCount; i++)
        {
            float y = i * (minY + maxY) / 2; // Average spacing increased due to larger minY/maxY
            if (i == 0) // Make sure the first platform is right below the player
            {
                SpawnPlatform(new Vector2(0, -1));
            }
            else
            {
                SpawnRandomPlatform(y);
            }
        }
    }

    void Update()
    {
        float cameraTop = mainCamera.transform.position.y + screenHeight / 2;
        // Increased buffer from 5f to 8f to spawn platforms less frequently
        if (highestPlatformY < cameraTop + 8f)
        {
            SpawnRandomPlatform(highestPlatformY + Random.Range(minY, maxY));
        }
        
        float cameraBottom = mainCamera.transform.position.y - screenHeight / 2;
        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            if (activePlatforms[i] != null && activePlatforms[i].transform.position.y < cameraBottom - 2f)
            {
                Destroy(activePlatforms[i]);
                activePlatforms.RemoveAt(i);
            }
        }
    }
    
    void SpawnRandomPlatform(float y)
    {
        float randomX = Random.Range(-screenWidth / 2 + 1, screenWidth / 2 - 1);
        Vector2 position = new Vector2(randomX, y);
        SpawnPlatform(position);
    }
    
    void SpawnPlatform(Vector2 position)
    {
        GameObject platform = Instantiate(platformPrefab, position, Quaternion.identity);
        
        PlatformEffector2D effector = platform.GetComponent<PlatformEffector2D>();
        if (effector == null)
        {
            effector = platform.AddComponent<PlatformEffector2D>();
        }
        
        effector.useOneWay = true;
        effector.useSideFriction = true;
        effector.surfaceArc = 180f;
        effector.rotationalOffset = 0f;
        
        activePlatforms.Add(platform);
        
        if (position.y > highestPlatformY)
        {
            highestPlatformY = position.y;
        }
        
        // Spawn special object
        if (Random.value < specialObjectSpawnChance)
        {
            Debug.Log("Attempting to spawn special object");
            Vector2 specialPos = new Vector2(position.x, position.y + 1f);
            if (specialObjectPrefab == null)
            {
                Debug.LogError("specialObjectPrefab is null!");
                return;
            }
            GameObject specialObj = Instantiate(specialObjectPrefab, specialPos, Quaternion.identity);
            if (specialObj.GetComponent<SpecialObjectCollision>() == null)
            {
                specialObj.AddComponent<SpecialObjectCollision>();
            }
        }
    }

    public void CleanupPlatforms()
    {
        // Destroy all active platforms
        foreach (GameObject platform in activePlatforms)
        {
            if (platform != null)
            {
                Destroy(platform);
            }
        }
        
        // Clear the list
        activePlatforms.Clear();
        
        // Destroy any remaining collectibles
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        foreach (GameObject collectible in collectibles)
        {
            if (collectible != null)
            {
                Destroy(collectible);
            }
        }
    }
}