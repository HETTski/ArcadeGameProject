using UnityEngine;
using TMPro; // Wymagane do obs³ugi TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Wzorzec Singleton dla ³atwego dostêpu

    [Header("Zasoby gracza")]
    public int money = 20; // Kieszonkowe na start
    public int tickets = 0;

    [Header("UI References")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI ticketsText;

    void Awake()
    {
        // Prosty Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    // Funkcja wywo³ywana przez automaty
    public void PlayMinigame(int cost)
    {
        if (money >= cost)
        {
            money -= cost;
            // W prawdziwej grze tutaj ³adowa³aby siê scena z mini-gr¹.
            // Na potrzeby PoC symulujemy wynik od razu:
            int wonTickets = Random.Range(5, 21); // Losowa wygrana biletów
            tickets += wonTickets;

            Debug.Log($"Zagrano w grê! Koszt: {cost}, Wygrane bilety: {wonTickets}");
            UpdateUI();
        }
        else
        {
            Debug.Log("Za ma³o pieniêdzy!");
        }
    }

    private void UpdateUI()
    {
        if (moneyText != null) moneyText.text = $"Pieni¹dze: ${money}";
        if (ticketsText != null) ticketsText.text = $"Bilety: {tickets}";
    }
}