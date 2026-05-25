using UnityEngine;
using System.Collections;

public class LevelZeroTutorial : MonoBehaviour
{
    [Header("UI Samouczka")]
    public GameObject pressText;
    public GameObject rotateIcons; // Strzałki lewo/prawo
    public GameObject thrustIcon;  // Strzałka w górę
    public GameObject shootIcon;   // Spacja

    [Header("Referencje do gry")]
    public MonoBehaviour normalAsteroidSpawner;
    public GameObject tutorialAsteroidPrefab;

    private int tutorialStep = 0;
    private GameObject targetAsteroid;

    private void Start()
    {
        // Sprawdzamy, czy gracz już ukończył ten samouczek w przeszłości
        if (PlayerPrefs.GetInt("Tutorial_Space", 0) == 1)
        {
            EndTutorial();
            return;
        }

        // START POZIOMU 0
        pressText.SetActive(true);
        rotateIcons.SetActive(true);
        thrustIcon.SetActive(false);
        shootIcon.SetActive(false);

        // Wyłączamy spawner na czas samouczka
        if (normalAsteroidSpawner != null)
        {
            normalAsteroidSpawner.enabled = false;
        }
    }

    private void Update()
    {
        if (tutorialStep == 0)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                rotateIcons.SetActive(false);
                thrustIcon.SetActive(true);
                tutorialStep = 1;
            }
        }
        else if (tutorialStep == 1)
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                thrustIcon.SetActive(false);
                SpawnTarget(); // Spawnuje cel po prawej stronie
                shootIcon.SetActive(true);
                tutorialStep = 2;
            }
        }
        else if (tutorialStep == 2)
        {
            // Jeśli nasz cel został zniszczony przez laser
            if (targetAsteroid == null)
            {
                shootIcon.SetActive(false);
                pressText.SetActive(false);
                tutorialStep = 3;
                StartCoroutine(TransitionToRealGame());
            }
        }
    }

    private void SpawnTarget()
    {
        // POPRAWKA: Spawnujemy asteroidę po prawej stronie ekranu (X = 0.8, Y = 0.5 w przestrzeni kamery)
        // Dzięki temu jest daleko od środka i gracz musi się obrócić w jej stronę, aby oddać strzał!
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(0.8f, 0.5f, 10f));
        spawnPos.z = 0;

        targetAsteroid = Instantiate(tutorialAsteroidPrefab, spawnPos, Quaternion.identity);

        // Blokujemy jakikolwiek ruch fizyczny tej jednej testowej asteroidy
        Rigidbody2D rb = targetAsteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private IEnumerator TransitionToRealGame()
    {
        yield return new WaitForSeconds(1f);

        PlayerPrefs.SetInt("Tutorial_Space", 1);
        PlayerPrefs.Save();

        EndTutorial();
    }

    private void EndTutorial()
    {
        if (pressText != null) pressText.SetActive(false);

        // Odpalamy właściwą grę
        if (normalAsteroidSpawner != null)
        {
            normalAsteroidSpawner.enabled = true;
        }

        this.enabled = false;
    }
}