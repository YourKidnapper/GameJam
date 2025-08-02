using UnityEngine;

public class BetButton : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private SkillShopManager shopManager;
    [SerializeField] private GameObject betButton;      // ця ж кнопка
    [SerializeField] private GameObject toBattleButton; // кнопка, яка має з’явитися

    [SerializeField] private Animator CatAnimator;
    public void OnClick()
{
    int coins = coinSpawner.GetCoinCount();

    if (coins > 0)
    {
        dialogueManager.ShowMessage($"{coins} монет – приймаю. Ось твій скіл!", true);
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
        dialogueManager.ShowMessage("Ти нічого не поставив!");
    }
}
}