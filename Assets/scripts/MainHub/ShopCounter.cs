using UnityEngine;

public class ShopCounter : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (GameManager.Instance.currentTickets >= GameManager.Instance.consoleCost)
        {
            // Odpalamy zwycięstwo!
            GameManager.Instance.TriggerEndgame(true);
        }
        else
        {
            int missing = GameManager.Instance.consoleCost - GameManager.Instance.currentTickets;
            // Używamy nowej, bezpiecznej metody z GameManagera!
            GameManager.Instance.BroadcastMessage($"Brakuje ci {missing} biletów do konsoli.");
        }
    }

    public string GetPromptText()
    {
        return $"Sklep z nagrodami (Konsola: {GameManager.Instance.consoleCost} biletów)";
    }
}