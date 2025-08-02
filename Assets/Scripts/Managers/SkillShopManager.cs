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

    [SerializeField] private AudioClip[] catVoiceClips;
    [SerializeField] private AudioSource catVoiceSource;

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
        // 🔍 Перевіряємо чи взагалі є доступні скіли
        var remainingSkills = allShopSkills
            .Where(s => !PlayerData.Instance.ownedSkills.Contains(s))
            .ToList();

        if (remainingSkills.Count == 0)
        {
                // Взагалі нічого не залишилось
                FindFirstObjectByType<DialogueManager>()
                    .ShowMessage("I already helped you enough. You should be able to handle it by yourself.");
                SoundFXManager.instance?.PlaySoundEffect(catVoiceClips[0], transform, 0.8f);
            return;
        }

        int targetRarity = GetRarityByBet(currentBet);

        // Фільтруємо за поточною ставкою
        var availableForBet = remainingSkills
            .Where(s => s.rarity == targetRarity)
            .ToList();

        if (availableForBet.Count == 0)
        {
            // Немає підходящих під цю ставку
            Debug.Log("❌ За цю ставку скілів більше нема");
            FindFirstObjectByType<DialogueManager>()
                .ShowMessage("I need more money to help you. Go and get!");
            SoundFXManager.instance?.PlaySoundEffect(catVoiceClips[0], transform, 0.8f);
            return;
        }

        if (currentBet >= 20)
        {
            SoundFXManager.instance?.PlaySoundEffect(catVoiceClips[1], transform, 0.8f);
        }

        // Основний вибір скіла
            SkillData chosen = availableForBet[Random.Range(0, availableForBet.Count)];

        PlayerData.Instance.AddSkill(chosen);
        allShopSkills.Remove(chosen);
        AddSkillToPanel(chosen);

        Debug.Log($"✅ Додано скіл: {chosen.skillName}");

        currentBet = 0;
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
