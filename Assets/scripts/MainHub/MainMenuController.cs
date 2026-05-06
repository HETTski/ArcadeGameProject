using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartNewGame()
    {
        // Ustawiamy flagę "0", co oznacza, że zaczynamy od zera
        PlayerPrefs.SetInt("LoadGameFlag", 0);
        SceneManager.LoadScene("MainHub");
    }

    public void LoadSavedGame()
    {
        // Sprawdzamy, czy w ogóle jest jakikolwiek zapis
        if (PlayerPrefs.HasKey("Weekend"))
        {
            // Ustawiamy flagę "1", żeby GameManager wiedział, że ma załadować dane
            PlayerPrefs.SetInt("LoadGameFlag", 1);
            SceneManager.LoadScene("MainHub");
        }
        else
        {
            Debug.Log("Brak zapisanego stanu gry!");
            // Tu opcjonalnie możesz wyświetlić jakiś tekst UI "Brak zapisu!"
        }
    }

    public void QuitGame()
    {
        Debug.Log("Wychodzę z gry!");
        Application.Quit(); // Ta funkcja zamyka grę (działa dopiero po zbudowaniu pliku .exe)
    }
}