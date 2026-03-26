using UnityEngine;
using UnityEngine.SceneManagement; // Niezbêdne do zmiany scen!

public class HubMachine : MonoBehaviour, IInteractable
{
    public string machineName = "Space Invaders";
    public int playCost = 2;
    public string sceneToLoad = "Minigame_SpaceInvaders"; // Nazwa sceny, któr¹ chcemy wczytaæ

    public void Interact()
    {
        if (GameManager.Instance.currentMoney >= playCost)
        {
            GameManager.Instance.SpendMoney(playCost);

            // Wysy³amy komunikat i ³adujemy scenê!
            GameManager.Instance.BroadcastMessage($"£adowanie gry: {machineName}...");

            // Wczytanie nowej sceny po nazwie
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            GameManager.Instance.BroadcastMessage("Nie staæ ciê na tê grê!");
        }
    }

    public string GetPromptText()
    {
        return $"Zagraj w {machineName} (${playCost})";
    }
}