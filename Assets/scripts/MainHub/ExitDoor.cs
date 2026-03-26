using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        GameManager.Instance.EndWeekend();
    }

    public string GetPromptText()
    {
        return "Wróæ do domu (Zakoñcz weekend)";
    }
}