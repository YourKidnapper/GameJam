using UnityEngine;
using TMPro;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private Collider2D spawnArea;

    private bool isSpawningBlocked = false;

    [Header("Ставки")]
    [SerializeField] private int[] thresholds = { 5, 10, 20 };
    [SerializeField]
    private string[] comments = {
        "Хмм, мало монет...",
        "О, вже щось цікавіше!",
        "Оце щедро!"
    };

    [Header("Параметри потоку")]
    [SerializeField] private float spawnDelay = 0.1f;
    private float spawnTimer;

    private int coinCount = 0;
    private int currentCommentIndex = 0;

    void Update()
    {
        if (!dialogueManager.isInGambleMode)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            TrySpawnCoin();
            spawnTimer = 0f;
        }

        if (Input.GetMouseButton(0))
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnDelay)
            {
                TrySpawnCoin();
                spawnTimer = 0f;
            }
        }
    }

    void TrySpawnCoin()
    {
        if (isSpawningBlocked) return;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;

        if (spawnArea.OverlapPoint(worldPos))
        {
            SpawnCoin(worldPos);
        }
    }

    void SpawnCoin(Vector3 pos)
    {
        GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);

        var sr = coin.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sortingLayerName = "Foreground";

        coinCount++;
        counterText.text = "Монет: " + coinCount;

        CheckThresholds();
    }

    void CheckThresholds()
    {
        if (currentCommentIndex < thresholds.Length &&
            coinCount >= thresholds[currentCommentIndex])
        {
            dialogueManager.ShowMessage(comments[currentCommentIndex], true);
            currentCommentIndex++;
        }
    }

    // ✅ Метод для кнопки
    public int GetCoinCount()
    {
        return coinCount;
    }

    // ✅ Скидання монет після ставки
    public void ResetCoins()
    {
        coinCount = 0;
        currentCommentIndex = 0;
        counterText.text = "Монет: 0";
    }

    public void BlockSpawning()
    {
    isSpawningBlocked = true;
    }
}
