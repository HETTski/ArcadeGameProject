using UnityEngine;
using UnityEngine.SceneManagement; 

public class ArcadeMachine : MonoBehaviour
{
    public int playCost = 2;
    public GameObject promptUI;
    private bool isPlayerInRange = false;

    void Start()
    {
        if (promptUI != null) promptUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            
            if (GameManager.Instance.money >= playCost)
            {
                GameManager.Instance.money -= playCost;
                SceneManager.LoadScene("Minigame1"); 
            }
            else
            {
                Debug.Log("Za ma³o pieniêdzy!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { isPlayerInRange = true; if (promptUI != null) promptUI.SetActive(true); }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { isPlayerInRange = false; if (promptUI != null) promptUI.SetActive(false); }
    }
}