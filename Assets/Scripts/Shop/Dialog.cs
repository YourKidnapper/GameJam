using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Animator CatAnimator; // додай сюди аніматор кота

    [Header("Dialogue Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    public bool isInGambleMode { get; private set; } = false;
    private bool dialogueWasFinished = false;

    [Header("Dialogue Lines")]
    [TextArea(3, 5)]
    [SerializeField]
    private string[] sentences = {
        "Вітаю, довбойоб мій любий друже!",
        "Цього разу я дам тобі нову зброю...",
        "Не підведи мене!"
    };

    [Header("Events")]
    public UnityEvent onDialogueFinished;

    private int index;
    private bool isTyping;

    private void Start()
    {
        dialoguePanel.SetActive(true);
        index = 0;
        StartCoroutine(TypeSentence(sentences[index]));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isInGambleMode)
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = sentences[index];
                isTyping = false;
                CatAnimator.SetBool("IsTalking", false); // вимкнути говоріння
            }
            else
            {
                NextSentence();
            }
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        // запуск анімації говоріння
        CatAnimator.SetBool("IsTalking", true);

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // завершення анімації
        CatAnimator.SetBool("IsTalking", false);

        isTyping = false;
    }

    private void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            StartCoroutine(TypeSentence(sentences[index]));
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