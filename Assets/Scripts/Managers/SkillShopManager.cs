using UnityEngine;
using System.Collections.Generic;

public class SkillShopManager : MonoBehaviour
{
    public List<SkillData> allShopSkills;
    public Transform shopPanel;
    public GameObject skillCardPrefab;
    public int playerMoney;

    void Start()
    {
        ShowRandomSkills(3); // Показуємо 3 картки
    }

    void ShowRandomSkills(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, allShopSkills.Count);
            SkillData skill = allShopSkills[index];
            GameObject card = Instantiate(skillCardPrefab, shopPanel);

            card.GetComponent<SkillCardUI>().Setup(skill, this);
        }
    }

    public void BuySkill(SkillData skill)
    {
        if (playerMoney >= skill.price)
        {
            playerMoney -= skill.price;
            PlayerData.Instance.AddSkill(skill);
            Debug.Log($"Куплено скіл: {skill.skillName}");
        }
        else
        {
            Debug.Log("Недостатньо грошей!");
        }
    }
}
