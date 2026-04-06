using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Endgame UI")]
    public GameObject endgamePanel;
    public TextMeshProUGUI endgameText;

    [Header("Referencje UI")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI ticketsText;
    public TextMeshProUGUI weekendText;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI interactionPromptText;

    [Header("Ustawienia")]
    [Tooltip("Ile sekund g³ówny komunikat ma wisieæ na ekranie?")]
    public float messageDuration = 3f; 

    private Coroutine messageCoroutine;

    private void Start()
    {
        GameManager.Instance.OnResourceChanged += UpdateUI;
        GameManager.Instance.OnGameMessage += ShowMessage;

        GameManager.Instance.OnGameOver += ShowEndgameScreen;
        if (endgamePanel != null) endgamePanel.SetActive(false);

        UpdateUI();
        HideInteractionPrompt();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnResourceChanged -= UpdateUI;
            GameManager.Instance.OnGameMessage -= ShowMessage;

            GameManager.Instance.OnGameOver -= ShowEndgameScreen;
        }
    }

    private void UpdateUI()
    {
        moneyText.text = $"Kieszonkowe: ${GameManager.Instance.currentMoney}";
        ticketsText.text = $"Bilety: {GameManager.Instance.currentTickets}";
        weekendText.text = $"Weekend: {GameManager.Instance.currentWeekend} / {GameManager.Instance.maxWeekends}";
    }

    private void ShowMessage(string msg)
    {
        if (messageText != null)
        {
            messageText.text = msg;

            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }

            // Zmieniliœmy sztywne 3f na Twoj¹ now¹ zmienn¹!
            messageCoroutine = StartCoroutine(HideMessageAfterDelay(messageDuration));
        }
        Debug.Log(msg);
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    public void ShowInteractionPrompt(string prompt)
    {
        interactionPromptText.text = $"[E] {prompt}";
        interactionPromptText.gameObject.SetActive(true);
    }

    public void HideInteractionPrompt()
    {
        interactionPromptText.gameObject.SetActive(false);
    }
    public void ClearMessage()
    {
        if (messageText != null)
        {
            messageText.text = "";
        }
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }
    }
    private void ShowEndgameScreen(bool isWin)
    {
        if (endgamePanel == null || endgameText == null) return;

        endgamePanel.SetActive(true);

        if (isWin)
        {
            endgameText.text = "GRATULACJE!\nUzbiera³eœ bilety i kupi³eœ wymarzon¹ konsolê!";
            endgameText.color = Color.green;
        }
        else
        {
            endgameText.text = "KONIEC LATA...\nNiestety, zabrak³o Ci biletów na konsolê.";
            endgameText.color = Color.red;
        }
    }

    // Funkcja dla przycisku Restart (aby mo¿na by³o go podpi¹æ w Unity)
    public void OnRestartButtonClicked()
    {
        GameManager.Instance.RestartGame();
    }

}