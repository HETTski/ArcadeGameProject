using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }

    [Header("UI Koñca Gry")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI resultText;

    private bool isGameOver = false;

    public void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);       
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("MainHub");
        }
        
    }

    public void TriggerGameOver()
    {
        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        if (resultText != null)
        {
            if (GameManager.Instance != null)
            {
                resultText.text = $"STATEK ZNISZCZONY!\nMasz ju¿ ³¹cznie: {GameManager.Instance.currentTickets} biletów. \n\n Wciœnij ENTER aby wróciæ do salonu";
            }
            else
            {
                resultText.text = "STATEK ZNISZCZONY!\n\nWciœnij ENTER aby wróciæ do salonu.";
            }
        }
    }
}
