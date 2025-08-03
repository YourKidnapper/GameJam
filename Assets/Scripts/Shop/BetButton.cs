using UnityEngine;

public class BetButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private SkillShopManager shopManager;

    [Header("UI Buttons")]
    [SerializeField] private GameObject betButton;       // ця ж кнопка
    [SerializeField] private GameObject toBattleButton;  // кнопка, яка має з’явитися

    [Header("Gamble UI")]
    [SerializeField] private GameObject bowl;
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject betCounter;
    [SerializeField] private GameObject spawnArea;

    [Header("Cat Animator")]
    [SerializeField] private Animator CatAnimator;

    public void OnClick()
    {
        int coins = coinSpawner.GetCoinCount();

        if (coins > 0)
        {
            dialogueManager.ShowMessage($"{coins} coins – I accept. Here's your skill!", true);
            shopManager.GiveSkillByBet(coins);
            coinSpawner.ResetCoins();
            coinSpawner.BlockSpawning();

            // Ховаємо цю кнопку та показуємо іншу
            betButton.SetActive(false);
            toBattleButton.SetActive(true);

            // Запускаємо анімацію видачі один раз
            CatAnimator.SetTrigger("Give");
        }
        else
        {
            // Ставка 0 – показуємо повідомлення та повертаємо до вибору
            dialogueManager.ShowMessage("You didn't bet anything!");

            // Повертаємо UI у попередній стан
            if (bowl != null) bowl.SetActive(false);
            if (betCounter != null) betCounter.SetActive(false);
            if (spawnArea != null) spawnArea.SetActive(false);
            if (betButton != null) betButton.SetActive(false);
            if (buttonsPanel != null) buttonsPanel.SetActive(true);

            dialogueManager.ExitGambleMode();
        }
    }
}
