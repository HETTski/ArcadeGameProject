using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
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

        UpdateUI();
        HideInteractionPrompt();
    }

    private void OnDestroy()
    {
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
}