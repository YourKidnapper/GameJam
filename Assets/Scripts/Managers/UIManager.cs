using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Health Bars")]
    public HealthBarUI playerHealthBar;
    public HealthBarUI enemyHealthBar;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void InitHealthBars(HealthSystem playerHealth, HealthSystem enemyHealth)
    {
        playerHealthBar.Setup(playerHealth);
        enemyHealthBar.Setup(enemyHealth);
    }
}
