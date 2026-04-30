using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformerController : MonoBehaviour
{
    [Header("Poruszanie")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Wykrywanie Ziemi")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // POPRAWKA 1: Używamy klasy Input, a nie zmiennej horizontalInput
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
    // 1. Zderzenie twarde (Fizyczne) - dla Beczek
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Barrel"))
        {
            // Gracz dotknął beczki -> Porażka, powrót z niczym
            if (MinigameManager.Instance != null)
            {
                MinigameManager.Instance.TriggerGameOver();
            }
            Destroy(gameObject); // Niszczymy gracza
        }
    }

    // 2. Zderzenie przenikające (Trigger) - dla Mety
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
           
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddTickets(500); // Główna wygrana: 500 biletów!
            }
            
            if (MinigameManager.Instance != null)
            {
                MinigameManager.Instance.TriggerWin();
            }
            
           
            gameObject.SetActive(false); 
        }
    }
}