// CameraFollow.cs
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public float smoothSpeed = 0.125f;
    public float verticalOffset = 2f;
    
    [Header("Debug")]
    public bool showDebugLogs = true; // Set to true by default for debugging
    
    private Transform target;
    private float highestYPos = 0f;
    private Camera mainCamera;
    private bool isFollowing = false;
    private Vector3 initialPosition;
    private float initialOrthographicSize;
    private GameObject jumpGame;
    
    void Awake()
    {
        Debug.Log("CameraFollow Awake called");
        
        // Get the main camera instead of requiring a camera component
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("CameraFollow - No main camera found in scene!");
            return;
        }
        
        // Store initial camera settings
        initialPosition = mainCamera.transform.position;
        initialOrthographicSize = mainCamera.orthographicSize;
        
        Debug.Log($"CameraFollow Awake - Initial Position: {initialPosition}, Size: {initialOrthographicSize}");
    }
    
    void Start()
    {
        Debug.Log("CameraFollow Start called");
        FindJumpGame();
    }
    
    private void FindJumpGame()
    {
        jumpGame = GameObject.FindGameObjectWithTag("JumpGame");
        if (jumpGame == null)
        {
            Debug.Log("JumpGame not found in scene - CameraFollow will be inactive");
        }
        else
        {
            Debug.Log($"Found JumpGame: {jumpGame.name}");
        }
    }
    
    private void FindPlayer()
    {
        // Only look for player if JumpGame exists
        if (jumpGame == null) return;

        // Find the player if not already assigned
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                Debug.Log($"CameraFollow - Found player: {player.name} at position {player.transform.position}");
            }
            else
            {
                Debug.LogError("CameraFollow - No player found with tag 'Player'");
                // Try to find any object with the Player tag
                GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
                Debug.Log($"Total objects in scene: {allObjects.Length}");
                foreach (GameObject obj in allObjects)
                {
                    Debug.Log($"Object: {obj.name}, Tag: {obj.tag}");
                }
            }
        }
    }
    
    public void StartFollowing()
    {
        Debug.Log("StartFollowing called");
    
        // Reset camera to initial position first
        ResetCamera();
    
        // Find JumpGame - try harder to find it
        FindJumpGame(); 
    
        if (jumpGame != null)
        {
            // Force find player again
            target = null;
            FindPlayer();
        
            isFollowing = true;
            highestYPos = 0f; // Reset highest Y position when starting to follow
            Debug.Log("Camera following started - JumpGame is active");
        }
        else
        {
            Debug.LogWarning("Cannot start following - JumpGame not found");
        }
    }
    
    public void StopFollowing()
    {
        Debug.Log("StopFollowing called");
        isFollowing = false;
        ResetCamera();
    }
    
    void LateUpdate()
    {
        // Check if JumpGame exists
        if (jumpGame == null)
        {
            FindJumpGame();
            if (jumpGame == null) return;
        }

        // Always try to find the player if we don't have a target
        if (target == null)
        {
            FindPlayer();
        }

        if (target == null || mainCamera == null)
        {
            if (!isFollowing) Debug.Log("Not following");
            if (target == null) Debug.Log("Target is null");
            if (mainCamera == null) Debug.Log("Main camera is null");
            return;
        }
        
        // Camera only follows player upward, never down
        if (target.position.y > highestYPos)
        {
            highestYPos = target.position.y;
            
            // Calculate desired position (only changing Y, keeping X the same)
            Vector3 desiredPosition = new Vector3(
                mainCamera.transform.position.x,
                highestYPos + verticalOffset,
                mainCamera.transform.position.z
            );
            
            // Smoothly move the camera
            Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, desiredPosition, smoothSpeed);
            mainCamera.transform.position = smoothedPosition;
            
            Debug.Log($"CameraFollow - Following player. Y: {highestYPos}, Camera Y: {mainCamera.transform.position.y}");
        }
    }
    
    private void ResetCamera()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = initialPosition;
            mainCamera.orthographicSize = initialOrthographicSize;
            highestYPos = 0f;
            
            Debug.Log($"CameraFollow - Camera reset to initial position: {initialPosition}");
        }
    }
}