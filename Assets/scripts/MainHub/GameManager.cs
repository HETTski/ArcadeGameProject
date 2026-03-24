using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Ekonomia")]
    public int currentMoney;
    public int currentTickets;
    public int allowancePerWeekend = 20; // Ile kieszonkowego dostajemy co tydzieñ
    public int consoleCost = 5000;       // Cel gry

    [Header("System Czasu")]
    public int currentWeekend = 1;
    public int maxWeekends = 10;

    // Zdarzenia (Events), do których podepnie siê UI
    public event Action OnResourceChanged;
    public event Action<string> OnGameMessage; // Do wysy³ania komunikatów (np. "Brak kasy!")

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
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
        currentWeekend++;

        if (currentWeekend > maxWeekends)
        {
            // Tutaj w przysz³oœci odpalimy ekran koñcowy
            OnGameMessage?.Invoke("Koniec lata! Zobaczmy, czy masz konsolê...");
        }
        else
        {
            StartNewWeekend();
        }
    }
}