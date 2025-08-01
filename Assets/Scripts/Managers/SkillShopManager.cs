using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SkillShopManager : MonoBehaviour
{
    [Header("Skills Pool")]
    public List<SkillData> allShopSkills; // Усі можливі скіли магазину

    [Header("Player Panel")]
    public Transform playerSkillPanel; // панель внизу
    public GameObject skillUIPrefab;   // префаб однієї кнопки/іконки скіла

    private int currentBet = 0;

    void Start()
    {
        // 🔹 На старті показуємо вже куплені скіли
        foreach (SkillData skill in PlayerData.Instance.ownedSkills)
        {
            AddSkillToPanel(skill);
        }
    }

    // Викликається кнопкою Bet
    public void GiveSkillByBet(int coins)
    {
        if (coins <= 0)
        {
            Debug.Log("❌ Не поставлено жодної монети");
            return;
        }

        currentBet = coins;
        ConfirmBet();
    }

    private void ConfirmBet()
    {
        int targetRarity = GetRarityByBet(currentBet);

        var raritySkills = allShopSkills.Where(s => s.rarity == targetRarity).ToList();
        if (raritySkills.Count == 0)
        {
            Debug.LogWarning($"❌ Немає скілів з rarity {targetRarity}");
            return;
        }

        SkillData chosen = raritySkills[Random.Range(0, raritySkills.Count)];

        // Додаємо в PlayerData
        PlayerData.Instance.AddSkill(chosen);

        // Додаємо на панель
        AddSkillToPanel(chosen);

        Debug.Log($"✅ Додано скіл: {chosen.skillName}");

        currentBet = 0;
    }

    private int GetRarityByBet(int bet)
    {
        if (bet >= 100) return 3;  
        if (bet >= 50) return 2;   
        return 1;                  
    }

    private void AddSkillToPanel(SkillData skill)
    {
        GameObject skillGO = Instantiate(skillUIPrefab, playerSkillPanel);
        SkillUI ui = skillGO.GetComponent<SkillUI>();
        if (ui != null)
            ui.Setup(skill);
        else
            Debug.LogError("❌ На SkillUIPrefab відсутній компонент SkillUI!");
    }
}
