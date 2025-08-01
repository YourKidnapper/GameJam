using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Health Bars")]
    public HealthBarUI playerHealthBar;
    public HealthBarUI enemyHealthBar;

    [Header("Coins UI")]
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (PlayerData.Instance != null)
        {
            UpdateCoinsUI(PlayerData.Instance.coins);
            PlayerData.Instance.OnCoinsChanged += UpdateCoinsUI;
        }
    }

    private void OnDestroy()
    {
        if (PlayerData.Instance != null)
            PlayerData.Instance.OnCoinsChanged -= UpdateCoinsUI;
    }

    public void InitHealthBars(HealthSystem playerHealth, HealthSystem enemyHealth)
    {
        playerHealthBar.Setup(playerHealth);
        enemyHealthBar.Setup(enemyHealth);
    }

    public void UpdateCoinsUI(int coins)
    {
        if (coinText != null)
            coinText.text = coins.ToString();
    }
}
