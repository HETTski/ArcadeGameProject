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

    [Header("Ekonomia")]
    public int currentMoney;
    public int currentTickets;
    public int allowancePerWeekend = 20; // Ile kieszonkowego dostajemy co tydzieñ
    public int consoleCost = 5000;       // Cel gry

    [Header("System Czasu")]
    public int currentWeekend = 1;
    public int maxWeekends = 10;

    [Header("Stan Gry")]
    public bool isGameOver = false;

    // Zdarzenia (Events), do których podepnie siê UI
    public event Action OnResourceChanged;
    public event Action<string> OnGameMessage; // Do wysy³ania komunikatów (np. "Brak kasy!")
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

        StartNewWeekend();
    }

    public void StartNewWeekend()
    {
        currentMoney = allowancePerWeekend;
        OnResourceChanged?.Invoke();
        OnGameMessage?.Invoke($"Rozpoczêto weekend {currentWeekend} / {maxWeekends}!");
    }

    public void SpendMoney(int amount)
    {
        currentMoney -= amount;
        OnResourceChanged?.Invoke();

        // Jeœli skoñczy³y nam siê pieni¹dze na zero, automatycznie koñczymy weekend
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
            // Koniec czasu! Sprawdzamy czy nas staæ.
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

        // Odpalamy event, którego s³ucha UIManager
        OnGameOver?.Invoke(isWin);
    }
    public void RestartGame()
    {

        // 1. Resetujemy wartoœci do pocz¹tkowych
        currentMoney = 20; // Wpisz tu swoj¹ kwotê startow¹
        currentTickets = 0;
        currentWeekend = 1;
        isGameOver = false;

        // 2. Aktualizujemy UI
        OnResourceChanged?.Invoke();

        endgamePanel.SetActive(false);

        // 3. £adujemy od nowa Hub
        SceneManager.LoadScene("MainHub");
    }
    // Publiczna funkcja, której inne skrypty mog¹ u¿ywaæ do wysy³ania tekstu na ekran
    public void BroadcastMessage(string msg)
    {
        OnGameMessage?.Invoke(msg);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainHub")
        {
            // W³¹czamy gracza (jeœli istnieje)
            if (hubPlayer != null)
            {
                hubPlayer.SetActive(true);
            }

            // W³¹czamy UI i od razu czyœcimy wiadomoœci (w jednym bloku if!)
            if (hubUI != null)
            {
                hubUI.SetActive(true);
                hubUI.GetComponent<UIManager>().ClearMessage();
            }
        }
        else
        {
            // Wy³¹czamy gracza i UI w minigrach
            if (hubPlayer != null) hubPlayer.SetActive(false);
            if (hubUI != null) hubUI.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}