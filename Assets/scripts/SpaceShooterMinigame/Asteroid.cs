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

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.RegisterAsteroid();
        }

        // Losujemy kierunek i siłę obrotu na samym starcie
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.linearVelocity = randomDirection * speed;
        rb.angularVelocity = Random.Range(-100f, 100f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        // Debug.Log($"Asteroida dotknęła obiektu: {other.gameObject.name} z tagiem: {other.tag}");

        if (other.CompareTag("Laser"))
        {
            // 1. Niszczymy pocisk gracza
            Destroy(other.gameObject);

            // Przyznawanie biletów
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddTickets(ticketValue);
            }
            else
            {
                Debug.Log($"Zdobywasz +{ticketValue} biletów! (Test bez huba)");
            }

            // Rozpad na dwie mniejsze asteroidy
            if (size > 1)
            {
                if (smallerAsteroidPrefab != null)
                {
                    // Każda nowo stworzona asteroida sama wylosuje swój kierunek lotu w swoim Start()
                    Instantiate(smallerAsteroidPrefab, transform.position, Quaternion.identity);
                    Instantiate(smallerAsteroidPrefab, transform.position, Quaternion.identity);
                }
                else
                {
               
                    Debug.LogWarning($"UWAGA! Asteroida wielkości {size} nie ma przypisanego prefabu w polu 'Smaller Asteroid Prefab'!");
                }
            }

            if (MinigameManager.Instance != null)
            {
                MinigameManager.Instance.UnregisterAsteroid();
            }

            Destroy(gameObject);
        }
    }
}