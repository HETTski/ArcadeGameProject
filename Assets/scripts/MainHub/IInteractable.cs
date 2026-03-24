// To nie jest MonoBehaviour! To kontrakt dla innych skryptów.
public interface IInteractable
{
    void Interact();
    string GetPromptText(); // Tekst, który wyœwietli siê nad obiektem (np. "Graj za $2", "Sklep")
}