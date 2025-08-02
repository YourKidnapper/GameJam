using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using DG.Tweening;

public class SkillShopManager : MonoBehaviour
{
    [Header("Skills Pool")]
    public List<SkillData> allShopSkills;

    [Header("Player Panel")]
    public Transform playerSkillPanel;
    public GameObject skillUIPrefab;

    [Header("Cat Position")]
    public Transform catTransform; // позиція кота, звідки летить скіл

    private int currentBet = 0;

    void Start()
    {
        foreach (SkillData skill in PlayerData.Instance.ownedSkills)
            AddSkillToPanel(skill, instant: true);
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

        var raritySkills = allShopSkills.Where(s => s.rarity == targetRarity).ToList();
        if (raritySkills.Count == 0)
        {
            Debug.LogWarning($"❌ Немає скілів з rarity {targetRarity}");
            return;
        }

        SkillData chosen = raritySkills[Random.Range(0, raritySkills.Count)];
        PlayerData.Instance.AddSkill(chosen);

        AddSkillToPanel(chosen, instant: false);

        Debug.Log($"✅ Додано скіл: {chosen.skillName}");
        currentBet = 0;
    }

    private int GetRarityByBet(int bet)
    {
        if (bet >= 100) return 3;
        if (bet >= 50) return 2;
        return 1;
    }

    private void AddSkillToPanel(SkillData skill, bool instant)
    {
        // створюємо іконку на рівні Canvas
        GameObject skillGO = Instantiate(skillUIPrefab, playerSkillPanel.parent);
        SkillUI ui = skillGO.GetComponent<SkillUI>();
        if (ui != null)
            ui.Setup(skill);

        RectTransform rect = skillGO.GetComponent<RectTransform>();
        rect.position = catTransform.position; 
        rect.localScale = Vector3.one;

        if (instant)
        {
            rect.SetParent(playerSkillPanel, false);
            return;
        }

        // обчислюємо кінцеву позицію для нового елемента в панелі
        Vector3 targetPos = playerSkillPanel.GetChild(0).position; 
        if (playerSkillPanel.childCount > 0)
            targetPos = playerSkillPanel.GetChild(playerSkillPanel.childCount - 1).position + new Vector3(100, 0, 0); 
        else
            targetPos = playerSkillPanel.position;

        // середня точка для гарної дуги
        Vector3 midPoint = (rect.position + targetPos) / 2f + Vector3.up * 150f;

        // анімація польоту по кривій
        rect.DOPath(new Vector3[] { rect.position, midPoint, targetPos }, 1f, PathType.CatmullRom)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                // після анімації додаємо в панель
                rect.SetParent(playerSkillPanel, false);
                rect.localScale = Vector3.one;
                rect.position = targetPos;
            });
    }
}