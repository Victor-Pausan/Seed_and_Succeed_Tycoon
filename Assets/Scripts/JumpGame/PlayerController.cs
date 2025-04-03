using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 13f;
    public float jumpForce = 15f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public LayerMask platformLayerMask;
    
    private Rigidbody2D rb;
    private bool isJumping = false;
    private float moveInput;
    private Camera mainCamera;
    private float halfPlayerWidth;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        halfPlayerWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
    }
    
    void Update()
    {
        moveInput = Input.GetAxis("Horizontal") * moveSpeed;
        
        // Screen wrapping
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);
        if (viewPos.x < 0)
        {
            transform.position = mainCamera.ViewportToWorldPoint(new Vector3(1, viewPos.y, viewPos.z));
        }
        else if (viewPos.x > 1)
        {
            transform.position = mainCamera.ViewportToWorldPoint(new Vector3(0, viewPos.y, viewPos.z));
        }
        
        // Better jump physics
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Check if player is falling off the bottom of the screen
        if (transform.position.y < mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - 2)
        {
            Game_Manager.instance.GameOver();
        }
    }
    
    void FixedUpdate()
    {
        // Move the player horizontally
        rb.velocity = new Vector2(moveInput, rb.velocity.y);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // We don't need this check anymore as the platform effector handles one-way collision
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            Destroy(collision.gameObject);
            Game_Manager.instance.AddScore(10);
        }
    }
}