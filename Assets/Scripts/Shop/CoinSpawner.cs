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
        "Hmm, not much coin...",
        "Oh, something more interesting!",
        "That's generous!"
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
        if (PlayerData.Instance.coins <= 0)
        {
            dialogueManager.ShowMessage("You're out of coins! Get a job!", true);
            isSpawningBlocked = true;
            return;
        }

        if (!PlayerData.Instance.SpendCoins(1))
        {
            isSpawningBlocked = true;
            return;
        }

        GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);

        var sr = coin.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sortingLayerName = "Foreground";

        coinCount++;
        counterText.text = "Coins: " + coinCount;

        UIManager.Instance?.UpdateCoinsUI(PlayerData.Instance.coins);

        CheckThresholds();

        if (PlayerData.Instance.coins == 0)
        {
            isSpawningBlocked = true;
            dialogueManager.ShowMessage("You can't place imaginary coins here!", true);
        }
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

    public int GetCoinCount()
    {
        return coinCount;
    }

    public void ResetCoins()
    {
        coinCount = 0;
        currentCommentIndex = 0;
        counterText.text = "Coins: 0";
    }

    public void BlockSpawning()
    {
        isSpawningBlocked = true;
    }
    
    public void UnblockSpawning()
    {
        isSpawningBlocked = false;
    }
}
