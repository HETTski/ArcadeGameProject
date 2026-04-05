using System.IO;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ruch")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public float upperPosition = 0.5f;

    [Header("Interakcja")]
    public UIManager uiManager; // Przypisz w Inspektorze
    private IInteractable currentInteractable;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver) return;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Obs³uga interakcji
        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.isGameOver) return;

        // Oblicz now¹ pozycjê za pomoc¹ fizyki (MovePosition)
        Vector2 newPos = rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime;

        // Ogranicz wspó³rzêdn¹ Y do upperPosition
        if (newPos.y > upperPosition)
        {
            newPos.y = upperPosition;
        }

        rb.MovePosition(newPos);
    }

    // Wykrywanie stref interakcji (trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            uiManager.ShowInteractionPrompt(interactable.GetPromptText());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var interactable = collision.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
            uiManager.HideInteractionPrompt();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var interactable = collision.gameObject.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            uiManager.ShowInteractionPrompt(interactable.GetPromptText());
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        var interactable = collision.gameObject.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
            uiManager.HideInteractionPrompt();
        }
    }
}