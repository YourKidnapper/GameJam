using UnityEngine;
using TMPro;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private TextMeshProUGUI counterText;

    [Header("Ставки")]
    [SerializeField] private int[] thresholds = { 5, 10, 20 };
    [SerializeField] private string[] comments = {
        "Хмм, мало монет...",
        "О, вже щось цікавіше!",
        "Оце щедро!"
    };

    [Header("Параметри потоку")]
    [SerializeField] private float spawnDelay = 0.1f; // затримка між монетами
    private float spawnTimer;

    private int coinCount = 0;
    private int currentCommentIndex = 0;

    void Update()
    {
        if (!dialogueManager.isInGambleMode)
            return;

        // Одиночний клік
        if (Input.GetMouseButtonDown(0))
        {
            SpawnCoin();
            spawnTimer = 0f; // щоб одразу не додавало зайву монету
        }

        // Утримання кнопки
        if (Input.GetMouseButton(0))
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnDelay)
            {
                SpawnCoin();
                spawnTimer = 0f;
            }
        }
    }

    void SpawnCoin()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;

        GameObject coin = Instantiate(coinPrefab, worldPos, Quaternion.identity);

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
}

