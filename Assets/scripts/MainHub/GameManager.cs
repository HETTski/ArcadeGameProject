using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        isGameOver = true;
        OnGameOver?.Invoke(isWin);
    }
    public void RestartGame()
    {
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Publiczna funkcja, której inne skrypty mogł używać do wysyłania tekstu na ekran
    public void BroadcastMessage(string msg)
    {
        OnGameMessage?.Invoke(msg);
    }
}