using UnityEngine;

public class BetButton : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private SkillShopManager shopManager;
    [SerializeField] private GameObject betButton;      
    [SerializeField] private GameObject toBattleButton; 
    [SerializeField] private Animator CatAnimator;

    public void OnClick()
    {
        int coins = coinSpawner.GetCoinCount();

        if (coins > 0)
        {
            dialogueManager.ShowMessage($"{coins} coins – accepted. Here is your new power!", true);
            shopManager.GiveSkillByBet(coins);
            coinSpawner.ResetCoins();

            // Блокуємо спавн тільки якщо реально дали гроші
            coinSpawner.BlockSpawning();

            // Ховаємо цю кнопку та показуємо іншу
            betButton.SetActive(false);
            toBattleButton.SetActive(true);

            // Анімація кота
            CatAnimator.SetTrigger("Give");
        }
        else
        {
            dialogueManager.ShowMessage("I need your sacrifice!");
            toBattleButton.SetActive(true);

            // Дозволяємо знову кидати монети
            coinSpawner.UnblockSpawning();
        }
    }
}