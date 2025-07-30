using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [Header("References")]
    public Transform playerSkillPanel; // Панель куди будемо додавати UI скілів
    public GameObject skillUIPrefab;   // Префаб для SkillUI

    [Header("Skill Pools")]
    public List<SkillData> availableSkills;
    public List<SkillData> playerSkills = new List<SkillData>();

    [Header("UI")]
    public Button randomSkillButton;

    void Start()
    {
        // Відмалювати стартові скіли
        foreach (SkillData skill in playerSkills)
        {
            AddSkillToUI(skill);
        }
        randomSkillButton.onClick.AddListener(PickRandomSkill);
    }

    void PickRandomSkill()
    {
        if (availableSkills.Count == 0) return;

        int index = Random.Range(0, availableSkills.Count);
        SkillData selectedSkill = availableSkills[index];

        // Додати до гравця
        playerSkills.Add(selectedSkill);

        // Видалити з пулу
        availableSkills.RemoveAt(index);

        // Відобразити
        AddSkillToUI(selectedSkill);
    }

    void AddSkillToUI(SkillData skill)
    {
        GameObject skillGO = Instantiate(skillUIPrefab, playerSkillPanel);
        skillGO.transform.localScale = Vector3.one;

        SkillUI skillUI = skillGO.GetComponent<SkillUI>();
        skillUI.Setup(skill); // все робить всередині
    }

}
