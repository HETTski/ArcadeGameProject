using UnityEngine;

// To zabezpieczenie sprawi, że Unity samo doda Rigidbody2D, jeśli o nim zapomnisz
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    [Header("Ustawienia Asteroidy")]
    [Tooltip("Wielkość: 3 (Duża), 2 (Średnia), 1 (Mała)")]
    public int size = 3;

    [Tooltip("Ile biletów gracz dostanie za zniszczenie tej asteroidy?")]
    public int ticketValue = 5;

    [Tooltip("Prędkość lotu asteroidy")]
    public float speed = 2f;

    [Header("Rozpad")]
    [Tooltip("Prefab mniejszej asteroidy. Zostaw puste dla Małej (size = 1).")]
    public GameObject smallerAsteroidPrefab;

    [Header("Dźwięki")]
    public AudioClip explosionSound;

    private Rigidbody2D rb;

    private bool isDestroyed = false;

    private void Awake()
    {
        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.RegisterAsteroid();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();


        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.linearVelocity = randomDirection * speed;
        rb.angularVelocity = Random.Range(-100f, 100f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (isDestroyed) return;

        if (other.CompareTag("Laser"))
        {
            isDestroyed = true; // Zaznaczamy, że skała dostała

            // NATYCHMIASTOWE ROZBROJENIE LASERA: 
            // Wyłączamy kolider, żeby nie uderzył "na wylot" w nowo zespawnowane skały!
            other.GetComponent<Collider2D>().enabled = false;
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);

            // Przyznawanie biletów
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddTickets(ticketValue);
            }

            // Rozpad na dwie mniejsze asteroidy
            if (size > 1)
            {
                if (smallerAsteroidPrefab != null)
                {
                    Instantiate(smallerAsteroidPrefab, transform.position, Quaternion.identity);
                    Instantiate(smallerAsteroidPrefab, transform.position, Quaternion.identity);
                }
            }

            // Dźwięk wybuchu
            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            }

            // Usunięcie ze spisu skał
            if (MinigameManager.Instance != null)
            {
                MinigameManager.Instance.UnregisterAsteroid();
            }

            Destroy(gameObject);
        }
    }
    
}