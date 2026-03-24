using UnityEngine;

public class ShopCounter : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        int cost = GameManager.Instance.consoleCost;
        if (GameManager.Instance.currentTickets >= cost)
        {
            Debug.Log("ZWYCIÊSTWO! Kupi³eœ wymarzon¹ konsolê!");
            // W przysz³oœci: LoadScene("WinScreen");
        }
        else
        {
            int missing = cost - GameManager.Instance.currentTickets;
            Debug.Log($"Brakuje ci {missing} biletów do konsoli.");
        }
    }

    public string GetPromptText()
    {
        return $"Sklep z nagrodami (Konsola: {GameManager.Instance.consoleCost} biletów)";
    }
}