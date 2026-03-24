using UnityEngine;

public class HubMachine : MonoBehaviour, IInteractable
{
    public string machineName = "Space Invaders";
    public int playCost = 2;

    public void Interact()
    {
        if (GameManager.Instance.currentMoney >= playCost)
        {
            GameManager.Instance.SpendMoney(playCost);

            // SYMULACJA: Zamiast ³adowaæ scenê, losujemy bilety natychmiast
            int wonTickets = Random.Range(10, 50);
            GameManager.Instance.AddTickets(wonTickets);

            Debug.Log($"Zagrano w {machineName}! Wygrana: {wonTickets} biletów.");
        }
        else
        {
            Debug.Log("Nie staæ ciê!");
        }
    }

    public string GetPromptText()
    {
        return $"Zagraj w {machineName} (${playCost})";
    }
}