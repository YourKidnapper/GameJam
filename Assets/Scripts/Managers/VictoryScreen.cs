using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScreenManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup victoryCanvas;
    [SerializeField] private Image backgroundPanel;
    [SerializeField] private TextMeshProUGUI victoryText;

    [Header("Settings")]
    [SerializeField] private float fadeTime = 1.5f;
    [SerializeField] private float scaleTime = 1f;
    [SerializeField] private string shopSceneName = "ShopScene";

    [Header("Audio")]
    [SerializeField] private AudioClip victorySound;

    private AudioSource audioSource;

    private void Awake()
    {
        victoryCanvas.alpha = 0;
        victoryCanvas.gameObject.SetActive(false);
        victoryText.transform.localScale = Vector3.one * 0.8f;

        if (backgroundPanel != null)
            backgroundPanel.color = new Color(0, 0, 0, 0);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayVictoryScreen()
    {
        DialogueManager.justWonBattle = true; // ⚡ позначаємо, що була перемога

        victoryCanvas.gameObject.SetActive(true);

        // Плавне затемнення
        victoryCanvas.DOFade(1f, fadeTime).SetEase(Ease.InOutQuad);

        if (backgroundPanel != null)
            backgroundPanel.DOFade(0.85f, fadeTime);

        // Текст масштабується
        victoryText.transform.DOScale(1.2f, scaleTime).SetEase(Ease.OutBack).SetDelay(0.2f);

        // Програємо звук перемоги
        if (victorySound != null)
        {
            audioSource.clip = victorySound;
            audioSource.Play();

            // Після завершення звуку -> магазин
            Invoke(nameof(LoadShopScene), victorySound.length + 0.3f);
        }
        else
        {
            // Якщо немає музики, перейти після анімації
            Invoke(nameof(LoadShopScene), fadeTime + 1f);
        }
    }

    private void LoadShopScene()
    {
        SceneManager.LoadScene(shopSceneName);
    }
}