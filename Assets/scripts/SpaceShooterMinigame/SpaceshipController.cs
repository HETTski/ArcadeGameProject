using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent (typeof(Rigidbody2D))]
public class SpaceshipController : MonoBehaviour
{
    [Header("Ustawienia Ruchu")]
    [Tooltip("Siła ciągu silnika")]
    public float thrustSpeed = 300f;
    [Tooltip("Prędkość obracania statuku")]
    public float rotationSpeed = 500f;

    [Header("Ustawienai Strzelania")]
    [Tooltip("Prefab pocisku lasera")]
    public GameObject laserPrefab;
    [Tooltip("Prędkość pocisku lasera")]
    public float laserSpeed = 100f;
    [Tooltip("dziób statku")]
    public Transform firePoint;



    private Rigidbody2D rb;

    private float verticalInput;
    private float horizontalInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }
    private void FixedUpdate()
    {
        if (verticalInput > 0)
        {
            rb.AddForce(transform.up * thrustSpeed * Time.fixedDeltaTime);
        }

        float rotationAmount = -horizontalInput * rotationSpeed * Time.fixedDeltaTime;
        rb.angularVelocity = rotationAmount * 10f;
    }
    private void Shoot()
    {
        if (firePoint == null || laserPrefab == null) return;
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();
        laserRb.linearVelocity = transform.up * laserSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
            return;
        if (other.CompareTag("Asteroid"))
        {
            MinigameManager.Instance.TriggerGameOver();
            Destroy(gameObject);
        }

    }
}
