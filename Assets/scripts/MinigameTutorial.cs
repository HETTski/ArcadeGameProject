using UnityEngine;

public class MinigameTutorial : MonoBehaviour
{
    [Tooltip("Unikalna nazwa dla tej gry, np. Tutorial_Frogger, Tutorial_Space")]
    public string tutorialID = "Tutorial_MojaGra";
    public GameObject tutorialPanel;

    private void Start()
    {
        // Sprawdzamy, czy gracz gra w tę minigrę po raz pierwszy
        if (PlayerPrefs.GetInt(tutorialID, 0) == 0)
        {
            tutorialPanel.SetActive(true);
            Time.timeScale = 0f; // Zatrzymujemy fizykę i czas w grze (w tym licznik czasu!)
        }
        else
        {
            tutorialPanel.SetActive(false);
            Time.timeScale = 1f; // Upewniamy się, że czas płynie normalnie
        }
    }

    public void StartMinigame()
    {
        // Zapisujemy, że gracz ukończył ten konkretny samouczek
        PlayerPrefs.SetInt(tutorialID, 1);
        PlayerPrefs.Save();

        tutorialPanel.SetActive(false);
        Time.timeScale = 1f; // Odpalamy grę!
    }

    private void OnDestroy()
    {
        // Zabezpieczenie: jeśli scena zostanie zniszczona (np. gracz wyjdzie do Huba), 
        // przywracamy upływ czasu, żeby cała gra się nie zawiesiła.
        Time.timeScale = 1f;
    }
}