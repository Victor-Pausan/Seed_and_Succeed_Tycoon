using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject collectiblePrefab;
    public int initialPlatformCount = 13;
    public float minY = 0.5f;
    public float maxY = 3.0f;
    public float collectibleSpawnChance = 0.2f;
    
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
            float y = i * (minY + maxY) / 2;
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
        // Generate new platforms as camera moves up
        float cameraTop = mainCamera.transform.position.y + screenHeight / 2;
        if (highestPlatformY < cameraTop + 5f)
        {
            SpawnRandomPlatform(highestPlatformY + Random.Range(minY, maxY));
        }
        
        // Remove platforms that are below the camera view
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
        
        // Ensure platform has a PlatformEffector2D for one-way collision
        PlatformEffector2D effector = platform.GetComponent<PlatformEffector2D>();
        if (effector == null)
        {
            effector = platform.AddComponent<PlatformEffector2D>();
        }
        
        // Configure the effector to only allow collision from above
        effector.useOneWay = true;
        effector.useSideFriction = true;
        effector.surfaceArc = 180f;
        effector.rotationalOffset = 0f;
        
        activePlatforms.Add(platform);
        
        if (position.y > highestPlatformY)
        {
            highestPlatformY = position.y;
        }
        
        // Chance to spawn a collectible above the platform
        if (Random.value < collectibleSpawnChance)
        {
            Vector2 collectiblePos = new Vector2(position.x, position.y + 1f);
            Instantiate(collectiblePrefab, collectiblePos, Quaternion.identity);
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