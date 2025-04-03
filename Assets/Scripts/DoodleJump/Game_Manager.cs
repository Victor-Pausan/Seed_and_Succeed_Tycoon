using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;
    
    public Text scoreText;
    public Text highScoreText;
    public GameObject gameOverPanel;
    private PlatformGenerator platformGenerator;
    private CameraFollow cameraFollow;
    private bool isGameActive = true;

    public TextMeshProUGUI repaymentRatio;
    public TextMeshProUGUI coinsCollectedText;
    
    private int score = 0;
    private int highScore = 0;
    
    void Awake()
    {
        
        
        Debug.Log("Game_Manager Awake called");
        if (gameOverPanel == null)
        {
            Debug.LogError("Game Over Panel reference is not set in the Inspector!");
        }
        gameOverPanel.SetActive(false);
        if (instance == null)
        {
            instance = this;
            Debug.Log("Game_Manager instance set");
        }
        else
        {
            Destroy(gameObject);
        }
        
        // Load high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI();
    }

    void Start()
    {
        isGameActive = true;
        Debug.Log("Game_Manager Start called");
        
        // Find the PlatformGenerator in the scene
        platformGenerator = FindObjectOfType<PlatformGenerator>();
        if (platformGenerator == null)
        {
            Debug.LogError("PlatformGenerator not found in scene!");
        }
        else
        {
            Debug.Log("PlatformGenerator found");
        }

        // Find or create the CameraFollow component
        cameraFollow = FindObjectOfType<CameraFollow>();
        if (cameraFollow == null)
        {
            // Create a new GameObject with CameraFollow component
            GameObject cameraFollowObj = new GameObject("CameraFollow");
            cameraFollow = cameraFollowObj.AddComponent<CameraFollow>();
            Debug.Log("Created new CameraFollow component");
        }
        else
        {
            Debug.Log("Found existing CameraFollow component");
        }
    }

    void Update()
    {
        if (!isGameActive)
        {
            Debug.Log("Game is not active, skipping update");
            return;
        }
        
        repaymentRatio.text = $"Repayment Ratio: {GameManager.repaymentAmount} supr/s";
        coinsCollectedText.text = GameManager.collectedCoins.ToString();
        // Check if player is below camera view
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float cameraBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;
            float playerY = player.transform.position.y;
            
            Debug.Log($"Player Y: {playerY}, Camera Bottom: {cameraBottom}");
            
            if (playerY < cameraBottom)
            {
                Debug.Log($"Player fell below camera view - Game Over! Player Y: {playerY}, Camera Bottom: {cameraBottom}");
                GameOver();
            }
        }
        else
        {
            Debug.LogWarning("No player found with tag 'Player' in Update");
        }
        
        //check how many coins were collected
        if (GameManager.collectedCoins == 5f)
        {
            GameManager.repaymentAmount += 0.05f;
            GameManager.collectedCoins = 0;
        }
    }
    
    public void AddScore(int points)
    {
        score += points;
        
        // Calculate height-based score
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            int heightScore = Mathf.FloorToInt(player.transform.position.y);
            if (heightScore > score)
            {
                score = heightScore;
            }
        }
        
        UpdateScoreUI();
    }
    
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
        }
    }
    
    public void GameOver()
    {
        if(gameOverPanel != null)
            gameOverPanel.SetActive(true);
        if (platformGenerator != null)
        {
            platformGenerator.CleanupPlatforms();
        }
    }
    
    public void RestartGame()
    {
        Debug.Log("Restarting game");
        isGameActive = true;
        
        // Clean up any existing platforms first
        if (platformGenerator != null)
        {
            platformGenerator.CleanupPlatforms();
        }
        
        // Reset score
        score = 0;
        UpdateScoreUI();
        
        // Hide game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        // Reset player position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(0, -1, 0);
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }
            Debug.Log($"Player reset to position: {player.transform.position}");
        }
        else
        {
            Debug.LogError("No player found with tag 'Player' during restart!");
        }
        
        // Start camera following
        if (cameraFollow != null)
        {
            cameraFollow.StartFollowing();
            Debug.Log("Camera following started");
        }
        else
        {
            Debug.LogError("CameraFollow component is null!");
        }
        
        // Let PlatformGenerator create new platforms
        if (platformGenerator != null)
        {
            platformGenerator.InitializePlatforms();
        }
    }

    public bool IsGameActive()
    {
        return isGameActive;
    }

    // Call this when the game starts (e.g., when the start button is clicked)
    public void StartGame()
    {
        Debug.Log("Starting game");
        isGameActive = true;
        
        // Start camera following
        if (cameraFollow != null)
        {
            cameraFollow.StartFollowing();
            Debug.Log("Camera following started");
        }
        else
        {
            Debug.LogError("CameraFollow component is null!");
        }
    }
}