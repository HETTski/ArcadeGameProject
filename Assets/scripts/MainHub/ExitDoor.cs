using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractable
{
    [Header("Dźwięki")]
    public AudioClip doorSound;


    public void Interact()
    {
        AudioSource.PlayClipAtPoint(doorSound, transform.position);
        GameManager.Instance.EndWeekend();
    }

    public string GetPromptText()
    {
        return "Wróć do domu (Zakończ weekend)";
    }
}