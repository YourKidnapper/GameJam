using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup deathCanvas;
    [SerializeField] private Image backgroundPanel; // темна панель позаду тексту
    [SerializeField] private TextMeshProUGUI deathText;

    [Header("Settings")]
    [SerializeField] private float fadeTime = 1.5f;
    [SerializeField] private float scaleTime = 1f;
    [SerializeField] private string shopSceneName = "ShopScene";
    [SerializeField] private AudioClip deathSound;

    private bool canProceed = false;

    private void Awake()
    {
        deathCanvas.alpha = 0;
        deathCanvas.gameObject.SetActive(false);
        deathText.transform.localScale = Vector3.one * 0.8f;

        if (backgroundPanel != null)
            backgroundPanel.color = new Color(0, 0, 0, 0); // прозорий чорний
    }

    public void PlayDeathScreen()
    {
        SoundFXManager.instance?.PlaySoundEffect(deathSound, transform, 1f);
        deathCanvas.gameObject.SetActive(true);

        // Поява чорного фону
        deathCanvas.DOFade(1f, fadeTime).SetEase(Ease.InOutQuad);

        // Затінення панелі
        if (backgroundPanel != null)
        {
            backgroundPanel.DOFade(0.85f, fadeTime); // затемнення до 85% чорного
        }

        // Плавне збільшення тексту
        deathText.transform.DOScale(1.2f, scaleTime).SetEase(Ease.OutBack).SetDelay(0.2f);

        // Дозволити перехід після анімації
        DOVirtual.DelayedCall(fadeTime + 0.5f, () =>
        {
            canProceed = true;
        });
    }

    private void Update()
    {
        if (canProceed && Input.anyKeyDown)
        {
            SceneManager.LoadScene(shopSceneName);
        }
    }
}
