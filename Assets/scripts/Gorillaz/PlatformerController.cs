using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformerController : MonoBehaviour
{
    [Header("Poruszanie")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Wykrywanie Ziemi")]
    public transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;


    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }
}
