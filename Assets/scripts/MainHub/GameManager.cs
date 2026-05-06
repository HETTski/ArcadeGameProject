using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Referencje do ukrywania")]
    public GameObject hubPlayer; 
    public GameObject hubUI;     
    public GameObject endgamePanel;
    public UnityEngine.UI.Image fadeImage;

    [Header("Ekonomia")]
    public int currentMoney;
    public int currentTickets;
    public int allowancePerWeekend = 20; // Ile kieszonkowego dostajemy co tydzień
    public int consoleCost = 5000;       // Cel gry

    [Header("System Czasu")]
    public int currentWeekend = 1;
    public int maxWeekends = 10;

    [Header("Stan Gry")]
    public bool isGameOver = false;

    // Zdarzenia (Events), do których podepnie się UI
    public event Action OnResourceChanged;
    public event Action<string> OnGameMessage; // Do wysyłania komunikatów (np. "Brak kasy!")
    public event Action<bool> OnGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else Destroy(gameObject);
    }

    private System.Collections.IEnumerator Start()
    {
       yield return null;

        // Sprawdzamy flagę przekazaną z Menu Głównego
        if (PlayerPrefs.GetInt("LoadGameFlag", 0) == 1)
        {
            LoadGame(); // Ładujemy zapis!
            // Resetujemy flagę, by nie ładowało zapisu przy ewentualnym restarcie
            PlayerPrefs.SetInt("LoadGameFlag", 0); 
        }
        else
        {
            StartNewWeekend(); // Zaczynamy od zera
        }
    }

    public void StartNewWeekend()
    {
        currentMoney = allowancePerWeekend;
        OnResourceChanged?.Invoke();
        OnGameMessage?.Invoke($"Rozpoczęto weekend {currentWeekend} / {maxWeekends}!");
    }

    public void SpendMoney(int amount)
    {
        currentMoney -= amount;
        OnResourceChanged?.Invoke();

        // Jeśli skończyły nam się pieniądze na zero, automatycznie kończymy weekend
        if (currentMoney <= 0)
        {
            EndWeekend();
        }
    }

    public void AddTickets(int amount)
    {
        currentTickets += amount;
        OnResourceChanged?.Invoke();
    }

    public void EndWeekend()
    {
        if (currentWeekend >= maxWeekends)
        {
            // Koniec czasu! Sprawdzamy czy nas stać.
            TriggerEndgame(currentTickets >= consoleCost);
        }
        else
        {
            currentWeekend++;
            StartNewWeekend();
        }
    }

    public void TriggerEndgame(bool isWin)
    {
        if (isGameOver) return; // Zabezpieczenie

        isGameOver = true;

        // Odpalamy event, kt�rego s�ucha UIManager
        OnGameOver?.Invoke(isWin);
    }
    public void RestartGame()
    {

        // 1. Resetujemy warto�ci do pocz�tkowych
        currentMoney = 20; // Wpisz tu swoj� kwot� startow�
        currentTickets = 0;
        currentWeekend = 1;
        isGameOver = false;

        // 2. Aktualizujemy UI
        OnResourceChanged?.Invoke();

        endgamePanel.SetActive(false);

        // 3. �adujemy od nowa Hub
       LoadSceneWithFade("MainHub");
    }
    // Publiczna funkcja, której inne skrypty mogł używać do wysyłania tekstu na ekran
    public void BroadcastMessage(string msg)
    {
        OnGameMessage?.Invoke(msg);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainHub")
        {
            // W��czamy gracza (je�li istnieje)
            if (hubPlayer != null)
            {
                hubPlayer.SetActive(true);
            }

            // W��czamy UI i od razu czy�cimy wiadomo�ci (w jednym bloku if!)
            if (hubUI != null)
            {
                hubUI.SetActive(true);
                hubUI.GetComponent<UIManager>().ClearMessage();
            }
        }
        else
        {
            // Wy��czamy gracza i UI w minigrach
            if (hubPlayer != null) hubPlayer.SetActive(false);
            if (hubUI != null) hubUI.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void SaveGame()
    {
        PlayerPrefs.SetInt("Money", currentMoney);
        PlayerPrefs.SetInt("Tickets", currentTickets);
        PlayerPrefs.SetInt("Weekend", currentWeekend);
        PlayerPrefs.Save(); // Wymuszenie zapisu na dysk
        
        BroadcastMessage("Gra została zapisana!");
    }

    public void LoadGame()
    {
        // Sprawdzamy, czy istnieje w ogóle jakiś zapis
        if (PlayerPrefs.HasKey("Weekend"))
        {
            currentMoney = PlayerPrefs.GetInt("Money");
            currentTickets = PlayerPrefs.GetInt("Tickets");
            currentWeekend = PlayerPrefs.GetInt("Weekend");
            
            OnResourceChanged?.Invoke(); // Aktualizacja UI
            BroadcastMessage("Gra wczytana pomyślnie.");
        }
        else
        {
            BroadcastMessage("Brak zapisanego stanu gry.");
        }
    }
    // Funkcja wywoływana zamiast bezpośredniego SceneManager.LoadScene()
    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private System.Collections.IEnumerator FadeAndLoad(string sceneName)
    {
        if (fadeImage != null)
        {
            // 1. Włączamy obraz i blokujemy interakcje myszką na czas ładowania
            fadeImage.gameObject.SetActive(true);
            fadeImage.raycastTarget = true; 

            // Ściemnianie (Fade Out)
            float alpha = 0;
            while (alpha < 1f)
            {
                alpha += Time.deltaTime * 2f; 
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }

        // Ładowanie sceny, gdy ekran jest w 100% czarny
        SceneManager.LoadScene(sceneName);

        if (fadeImage != null)
        {
            // Rozjaśnianie (Fade In)
            float alpha = 1f;
            while (alpha > 0f)
            {
                alpha -= Time.deltaTime * 2f;
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            // 2. Odblokowujemy interakcje i chowamy czarny ekran, żeby nie wisiał w tle
            fadeImage.raycastTarget = false;
            fadeImage.gameObject.SetActive(false);
        }
    }
}