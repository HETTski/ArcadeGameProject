using UnityEngine;
using UnityEngine.SceneManagement; 

public class HubMachine : MonoBehaviour, IInteractable
{
    public string machineName = "Space Invaders";
    public int playCost = 2;
    public string sceneToLoad = "Minigame_SpaceInvaders"; // Nazwa sceny, którą chcemy wczyta

    public int requiredWeekend = 1; 

    [Header("Dźwięki")]
    public AudioClip machineSound;

    public void Interact()
    {

        if (GameManager.Instance.currentWeekend < requiredWeekend)
        {
            GameManager.Instance.BroadcastMessage($"Automat nieczynny! Będzie dostępny w {requiredWeekend} tygodniu .");
            return;
        }

        AudioSource.PlayClipAtPoint(machineSound, transform.position);

        if (GameManager.Instance.currentMoney >= playCost)
        {
            GameManager.Instance.SpendMoney(playCost);

            // Wysyłamy komunikat i ładujemy scenę!
            GameManager.Instance.BroadcastMessage($"Ładowanie gry: {machineName}...");

            // Wczytanie nowej sceny po nazwie
          GameManager.Instance.LoadSceneWithFade(sceneToLoad);
        }
        else
        {
            GameManager.Instance.BroadcastMessage("Nie stać cię na tą grę!");
        }
    }

    public string GetPromptText()
    {
        return $"Zagraj w {machineName} (${playCost})";
    }
}