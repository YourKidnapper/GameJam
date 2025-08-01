using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Викликати при натисканні кнопки "To Battle"
    public void LoadBattleScene()
    {
        SceneManager.LoadScene("BattleScene"); // заміни на точну назву твоєї сцени бою
    }

    public void LoadShopScene()
    {
        SceneManager.LoadScene("ShopScene"); // якщо буде повертатись назад
    }
}
