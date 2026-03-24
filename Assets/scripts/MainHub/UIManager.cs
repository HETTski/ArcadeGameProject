using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI ticketsText;
    public TextMeshProUGUI weekendText;
    public TextMeshProUGUI messageText; // Np. na œrodku ekranu na komunikaty
    public TextMeshProUGUI interactionPromptText; // Tekst "Wciœnij E, aby..."

    private void Start()
    {
        // Podpinamy siê pod zdarzenia z GameManagera
        GameManager.Instance.OnResourceChanged += UpdateUI;
        GameManager.Instance.OnGameMessage += ShowMessage;

        UpdateUI();
        HideInteractionPrompt();
    }

    private void OnDestroy()
    {
        // Zawsze odpinamy zdarzenia, ¿eby unikn¹æ wycieków pamiêci
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnResourceChanged -= UpdateUI;
            GameManager.Instance.OnGameMessage -= ShowMessage;
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
        if (messageText != null) messageText.text = msg;
        Debug.Log(msg);
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
}