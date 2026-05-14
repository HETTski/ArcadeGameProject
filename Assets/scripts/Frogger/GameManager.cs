using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Frogger
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private Home[] homes;
        [SerializeField] private Frogger frogger;
        [SerializeField] private Text timeText;
        [SerializeField] private Text livesText;
        [SerializeField] private Text scoreText;

        public int lives { get; private set; } = 3;
        public int score { get; private set; } = 0;
        public int time { get; private set; } = 30;

        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Start()
        {
            NewGame();
        }

        private void NewGame()
        {
            SetScore(0);
            SetLives(3); // Gracz ma 3 życia (próby) na żeton
            NewLevel();
        }

        private void NewLevel()
        {
            for (int i = 0; i < homes.Length; i++)
            {
                homes[i].enabled = false;
            }
            Respawn();
        }

        private void Respawn()
        {
            frogger.Respawn();
            StopAllCoroutines();
            StartCoroutine(Timer(30));
        }

        private IEnumerator Timer(int duration)
        {
            time = duration;
            timeText.text = time.ToString();

            while (time > 0)
            {
                yield return new WaitForSeconds(1);
                time--;
                timeText.text = time.ToString();
            }

            frogger.Death();
        }

        public void Died()
        {
            SetLives(lives - 1);

            if (lives > 0)
            {
                Invoke(nameof(Respawn), 1f);
            }
            else
            {
                Invoke(nameof(GameOver), 1f);
            }
        }

        private void GameOver()
        {
            frogger.gameObject.SetActive(false);
            StopAllCoroutines();

            // PRZEGRANA: Obliczamy bilety na podstawie zdobytych punktów (np. 1 bilet za każde 10 pkt)
            int earnedTickets = score / 10;

            // Komunikacja z GŁÓWNYM GameManagerem z Huba
            if (global::GameManager.Instance != null)
            {
                global::GameManager.Instance.AddTickets(earnedTickets);
            }

            // Wywołanie ekranu końcowego i wyjście do Huba
            if (MinigameManager.Instance != null)
            {
                MinigameManager.Instance.TriggerGameOver();
            }
            else
            {
                Debug.LogWarning("Brakuje MinigameManagera na scenie Froggera!");
            }
        }

        public void AdvancedRow()
        {
            SetScore(score + 10);
        }

        public void HomeOccupied()
        {
            frogger.gameObject.SetActive(false);

            int bonusPoints = time * 20;
            SetScore(score + bonusPoints + 50);

            if (Cleared())
            {
                // WYGRANA: Gracz zajął wszystkie domki!
                if (global::GameManager.Instance != null)
                {
                    global::GameManager.Instance.AddTickets(500); // Główna wygrana!
                }

                if (MinigameManager.Instance != null)
                {
                    MinigameManager.Instance.TriggerWin();
                }
            }
            else
            {
                // Zajął jeden domek, gra toczy się dalej (żaba wraca na start)
                Invoke(nameof(Respawn), 1f);
            }
        }

        private bool Cleared()
        {
            for (int i = 0; i < homes.Length; i++)
            {
                if (!homes[i].enabled)
                {
                    return false;
                }
            }
            return true;
        }

        private void SetScore(int score)
        {
            this.score = score;
            if (scoreText != null) scoreText.text = score.ToString();
        }

        private void SetLives(int lives)
        {
            this.lives = lives;
            if (livesText != null) livesText.text = lives.ToString();
        }
    }
}