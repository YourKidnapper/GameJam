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
        // Показуємо вже куплені скіли
        foreach (SkillData skill in PlayerData.Instance.ownedSkills)
        {
            AddSkillToPanel(skill);
        }
    }

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

        // Підбираємо скіл з урахуванням fallback
        SkillData chosen = GetSkillWithFallback(targetRarity);

        if (chosen == null)
        {
            Debug.LogWarning("❌ В магазині більше немає доступних скілів!");
            return;
        }

        // Додаємо в PlayerData
        PlayerData.Instance.AddSkill(chosen);

        // Прибираємо скіл з магазину, щоб не дублювався
        allShopSkills.Remove(chosen);

        // Додаємо на панель
        AddSkillToPanel(chosen);

        Debug.Log($"✅ Додано скіл: {chosen.skillName}");

        currentBet = 0;
    }

    // Підбирає rarity з fallback
    private SkillData GetSkillWithFallback(int startingRarity)
    {
        int rarity = startingRarity;

        // Основний пошук: від запитаного до 1
        while (rarity >= 1)
        {
            var available = allShopSkills
                .Where(s => s.rarity == rarity && !PlayerData.Instance.ownedSkills.Contains(s))
                .ToList();

            if (available.Count > 0)
                return available[Random.Range(0, available.Count)];

            rarity--;
        }

        // 💫 Рідкісний шанс на кращий скіл (2%)
        if (Random.value <= 0.02f)
        {
            int maxRarity = allShopSkills.Max(s => s.rarity);
            var betterSkills = allShopSkills
                .Where(s => s.rarity > startingRarity && !PlayerData.Instance.ownedSkills.Contains(s))
                .ToList();

            if (betterSkills.Count > 0)
            {
                Debug.Log("🎉 Випала рідкісна удача! Отримано скіл вищої якості");
                return betterSkills[Random.Range(0, betterSkills.Count)];
            }
        }

        return null; // взагалі нічого не залишилось
    }

    private int GetRarityByBet(int bet)
    {
        if (bet >= 100) return 4;
        if (bet >= 50) return 3;  
        if (bet >= 30) return 2;   
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
