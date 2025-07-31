using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [Header("References")]
    public Transform playerSkillPanel;
    public GameObject skillUIPrefab;
    public GameObject player;

    [Header("Skill Logic")]
    public AttackSkill attackSkill;
    public SupportSkill supportSkill;
    public PassiveSkill passiveSkill;

    [Header("Skill Pools")]
    public List<SkillData> availableSkills;
    public List<SkillData> playerSkills = new List<SkillData>();

    [Header("UI")]
    public Button randomSkillButton;

    public HealthSystem playerHealthSystem;
    public HealthSystem enemyHealthSystem;

    public static SkillManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        // Прив’язуємо health-системи
        UIManager.Instance.InitHealthBars(playerHealthSystem, enemyHealthSystem);

        // Додаємо стартові скіли
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

        playerSkills.Add(selectedSkill);
        availableSkills.RemoveAt(index);

        AddSkillToUI(selectedSkill);
    }

    void AddSkillToUI(SkillData skill)
    {
        GameObject skillGO = Instantiate(skillUIPrefab, playerSkillPanel);
        skillGO.transform.localScale = Vector3.one;

        SkillUI skillUI = skillGO.GetComponent<SkillUI>();
        skillUI.Setup(skill);

        SkillButtonController btnController = skillGO.GetComponent<SkillButtonController>();
        if (btnController != null)
        {
            btnController.Setup(skill); // Вся логіка кліку — всередині
        }

        // Запустити пасивку, якщо треба
        if (skill.isPassive)
        {
            StartCoroutine(RunPassive(skill));
        }
    }


    void OnSkillClicked(SkillData skill)
    {
        GameObject target = FindTarget(); // Реалізуй сам або тимчасово зроби null

        switch (skill.type)
        {
            case SkillType.Attack:
                attackSkill.Activate(skill, player, target);
                break;

            case SkillType.Support:
                supportSkill.Activate(skill, player);
                break;

            case SkillType.Passive:
                passiveSkill.Activate(skill, player);
                break;
        }
    }

    public void DisableAllSkills(float duration)
    {
        StartCoroutine(DisableAllSkillsCoroutine(duration));
    }

    private IEnumerator DisableAllSkillsCoroutine(float duration)
    {
        // Знайди всі кнопки в playerSkillPanel
        Button[] buttons = playerSkillPanel.GetComponentsInChildren<Button>();

        foreach (var btn in buttons)
            btn.interactable = false;

        yield return new WaitForSeconds(duration);

        foreach (var btn in buttons)
            btn.interactable = true;
    }

    GameObject FindTarget()
    {
        return GameObject.FindWithTag("Enemy");
    }

    public void ActivateSkill(SkillData skill)
    {
        GameObject target = FindTarget();

        switch (skill.type)
        {
            case SkillType.Attack:
                attackSkill.Activate(skill, player, target);
                break;
            case SkillType.Support:
                supportSkill.Activate(skill, player);
                break;
            case SkillType.Passive:
                passiveSkill.Activate(skill, player);
                break;
        }
    }

    public void BlockSkillsDuringAnimation(float duration)
    {
        StartCoroutine(BlockCoroutine(duration));
    }

    private IEnumerator BlockCoroutine(float duration)
    {
        Button[] buttons = playerSkillPanel.GetComponentsInChildren<Button>();

        foreach (var btn in buttons)
            btn.interactable = false;

        yield return new WaitForSeconds(duration);

        foreach (var btn in buttons)
            btn.interactable = true;
    }

    
    private IEnumerator RunPassive(SkillData data)
    {
        while (true)
        {
            yield return new WaitForSeconds(data.cooldown);
            passiveSkill.Activate(data, player);  // уже реалізований метод
        }
    }
}
