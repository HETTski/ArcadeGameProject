using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }

    [Header("UI Koñca Gry")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI resultText;

    private bool isGameOver = false;

    private int asteroidCount = 0;

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

    public void RegisterAsteroid()
    {
        asteroidCount++;
    }

    public void UnregisterAsteroid()
    {
        asteroidCount--;

        // Jeœli to by³a ostatnia asteroida i gracz jeszcze ¿yje...
        if (asteroidCount <= 0 && !isGameOver)
        {
            TriggerWin();
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            gameOverPanel.GetComponent<Image>().color = new Color(0.5f, 0, 0, 0.8f);
        }
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
    public void TriggerWin()
    {
        isGameOver = true;

        gameOverPanel.SetActive(true);
        gameOverPanel.GetComponent<Image>().color = new Color(0, 0.5f, 0, 0.8f);
        resultText.text = $"ZWYCIÊSTWO!\nOczyœci³eœ sektor!\nBilety: {GameManager.Instance.currentTickets}\n\nWciœnij [ENTER] aby wróciæ.";
    }
}
