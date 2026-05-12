using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformerController : MonoBehaviour
{
    [Header("Poruszanie")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float climbSpeed = 5f; 

    [Header("Wykrywanie Ziemi")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Dźwięki")]
    public AudioClip jumpSound; 

    private Rigidbody2D rb;
    private float horizontalInput;
    private float verticalInput; 
    private bool isGrounded;
    
    // Zmienne do drabiny
    private bool isTouchingLadder; 
    private bool isClimbing;       
    private float defaultGravity;  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale; // Zapamiętujemy domyślną grawitację gracza
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical"); 

        // 1. Logika Drabiny
        if (isTouchingLadder && Mathf.Abs(verticalInput) > 0f)
        {
            isClimbing = true; 
        }

        
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || isClimbing))
        {
            isClimbing = false; 
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // Odtwarzanie dźwięku
            if (jumpSound != null)
            {
                AudioSource.PlayClipAtPoint(jumpSound, transform.position);
            }
        }
    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            gameObject.layer = LayerMask.NameToLayer("Climbing");

            rb.gravityScale = 0f; 
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, verticalInput * climbSpeed);
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");

            rb.gravityScale = defaultGravity; // Przywracamy domyślną grawitację
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    // --- ZABEZPIECZENIA I KOLIZJE ---

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Wykrycie Drabiny
        if (other.CompareTag("Ladder")) isTouchingLadder = true;

        // Meta (z poprzedniej lekcji)
        if (other.CompareTag("Goal"))
        {
            if (GameManager.Instance != null) GameManager.Instance.AddTickets(500);
            if (MinigameManager.Instance != null) MinigameManager.Instance.TriggerWin();
            gameObject.SetActive(false); 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Wyjście z Drabiny
        if (other.CompareTag("Ladder"))
        {
            isTouchingLadder = false;
            isClimbing = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Barrel"))
        {
            if (MinigameManager.Instance != null) MinigameManager.Instance.TriggerGameOver();
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}