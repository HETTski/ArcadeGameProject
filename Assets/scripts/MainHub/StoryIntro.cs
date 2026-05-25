using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class StoryIntro : MonoBehaviour
{
    [Header("UI References")]
    public GameObject introPanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public PlayerController player; // Do zablokowania ruchu

    [Header("Settings")]
    public float typingSpeed = 0.05f; // Szybkość wypisywania liter (im mniejsza wartość, tym szybciej)
    public AudioClip typingSound;     // Retro pikanie podczas pisania
    private AudioSource audioSource;
    private bool canInteract = false;

    // Struktura pojedynczej linijki dialogu
    [System.Serializable]
    public struct DialogueLine
    {
        public string speakerName;
        [TextArea(2, 5)]
        public string text;
    }

    [Header("Story Dialogue")]
    public List<DialogueLine> storyLines;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    private IEnumerator Start()
    {

        if (GameManager.Instance.currentWeekend == 1 && PlayerPrefs.GetInt("IntroSeen", 0) == 0)
        {
            introPanel.SetActive(true);
            if (player != null) player.enabled = false;


            yield return new WaitForSeconds(0.6f);

            canInteract = true; 
            StartDialogue();
        }
        else
        {
            introPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (introPanel.activeSelf)
        {
            // Kontynuacja po kliknięciu myszą, spacją lub Enterem
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                if (isTyping)
                {
                    // Jeśli gracz kliknął w trakcie wypisywania -> wyświetl całą linijkę od razu (skip)
                    CompleteTypingEarly();
                }
                else
                {
                    // Jeśli linijka się wypisała -> przejdź do następnej
                    NextLine();
                }
            }
        }
    }

    private void StartDialogue()
    {
        currentLineIndex = 0;
        DisplayLine();
    }

    private void DisplayLine()
    {
        if (currentLineIndex < storyLines.Count)
        {
            DialogueLine currentLine = storyLines[currentLineIndex];
            speakerNameText.text = currentLine.speakerName;

            // Kolorowanie imion (opcjonalne, ale daje fajny efekt!)
            if (currentLine.speakerName == "Syn") speakerNameText.color = Color.cyan;
            else if (currentLine.speakerName == "Mama") speakerNameText.color = new Color(1f, 0.5f, 0.8f); // Różowy/Fioletowy

            typingCoroutine = StartCoroutine(TypeSentence(currentLine.text));
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            // Dźwięk pikania przy każdej literce (poza spacjami)
            if (letter != ' ' && typingSound != null)
            {
                audioSource.PlayOneShot(typingSound, 0.5f);
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void CompleteTypingEarly()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        dialogueText.text = storyLines[currentLineIndex].text;
        isTyping = false;
    }

    private void NextLine()
    {
        currentLineIndex++;
        DisplayLine();
    }

    private void EndDialogue()
    {
        PlayerPrefs.SetInt("IntroSeen", 1);
        PlayerPrefs.Save();

        introPanel.SetActive(false);
        if (player != null) player.enabled = true;

        GameManager.Instance.BroadcastMessage("Cel: Zdobądź konsolę! (Poruszanie: WSAD)");
    }
}