using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Animator CatAnimator;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip catVoiceClip;

    [Header("Dialogue Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    public bool isInGambleMode { get; private set; } = false;
    private bool dialogueWasFinished = false;

    // 🔹 Перевірка чи це перший візит
    private static bool firstVisitDone = false;

    // 🔹 Перевірка чи гравець переміг у попередній сцені
    public static bool justWonBattle = false;

    [Header("First Time Dialogue")]
    [TextArea(3, 5)]
    [SerializeField] private string[] firstTimeSentences = {
        "Greetings! I'm Gambling King!",
        "But my friends may call me Fluffy",
        "But you're not my friend, of course",
        "So I can only help you for gold"
    };

    [Header("Repeat Dialogue")]
    [TextArea(3, 5)]
    [SerializeField] private string[] repeatSentences = {
        "You again? Let's see if you screw up this time..",
        "Will you make a sacrifice to the cat god?"
    };

    [Header("Victory Dialogue")]
    [TextArea(3, 5)]
    [SerializeField] private string[] victorySentences = {
        "Well done, champion!",
        "Your claws were sharp this time.",
        "This is the end of our game, but this loop can be repeated",
        "The cat gods planned to do a lot more",
        "But they couldn't in such a short time",
        "But I'm glad it all turned out the way it is",
        "Thank you for getting here"
    };

    [Header("Events")]
    public UnityEvent onDialogueFinished;

    private int index;
    private bool isTyping;
    private string[] currentSentences;

    private void Start()
    {
        dialoguePanel.SetActive(true);
        index = 0;

        // 🔹 Вибір набору реплік
        if (justWonBattle)
        {
            currentSentences = victorySentences;
            justWonBattle = false; // скидаємо прапорець
        }
        else if (!firstVisitDone)
        {
            currentSentences = firstTimeSentences;
            firstVisitDone = true;
        }
        else
        {
            currentSentences = repeatSentences;
        }

        StartCoroutine(TypeSentence(currentSentences[index]));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isInGambleMode)
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentSentences[index];
                isTyping = false;
                CatAnimator.SetBool("IsTalking", false);
            }
            else
            {
                NextSentence();
            }
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        if (catVoiceClip != null)
            SoundFXManager.instance?.PlaySoundEffect(catVoiceClip, transform, 0.5f);

        isTyping = true;
        dialogueText.text = "";

        CatAnimator.SetBool("IsTalking", true);

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        CatAnimator.SetBool("IsTalking", false);
        isTyping = false;
    }

    private void NextSentence()
    {
        if (index < currentSentences.Length - 1)
        {
            index++;
            StartCoroutine(TypeSentence(currentSentences[index]));
        }
        else if (!dialogueWasFinished)
        {
            dialogueWasFinished = true;
            onDialogueFinished?.Invoke();
        }
    }

    public void ShowMessage(string message, bool gambleMode = false)
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(message));
        isInGambleMode = gambleMode;
    }

    public void ExitGambleMode()
    {
        isInGambleMode = false;
    }
}
