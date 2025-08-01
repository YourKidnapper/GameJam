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

    [Header("Health Systems")]
    public HealthSystem playerHealthSystem;
    public HealthSystem enemyHealthSystem;

    public static SkillManager Instance { get; private set; }

    private bool isPlayerDead = false;
    private bool isEnemyDead = false;
    private List<SkillData> playerSkills = new List<SkillData>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Підключаємо хп-бари
        UIManager.Instance.InitHealthBars(playerHealthSystem, enemyHealthSystem);

        // Слухаємо смерть
        playerHealthSystem.OnDeath += OnPlayerDied;
        enemyHealthSystem.OnDeath += OnEnemyDied;

        // Підтягуємо скіли з PlayerData (отримані в магазині)
        if (PlayerData.Instance != null)
            playerSkills = new List<SkillData>(PlayerData.Instance.ownedSkills);

        // Створюємо UI-кнопки
        foreach (SkillData skill in playerSkills)
            AddSkillToUI(skill);
    }

    private void AddSkillToUI(SkillData skill)
    {
        GameObject skillGO = Instantiate(skillUIPrefab, playerSkillPanel);
        skillGO.transform.localScale = Vector3.one;

        SkillUI skillUI = skillGO.GetComponent<SkillUI>();
        skillUI.Setup(skill);

        SkillButtonController btnController = skillGO.GetComponent<SkillButtonController>();
        if (btnController != null)
            btnController.Setup(skill);

        if (skill.isPassive)
            StartCoroutine(RunPassive(skill));
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

    GameObject FindTarget()
    {
        return GameObject.FindWithTag("Enemy");
    }

    public void DisableAllSkills(float duration)
    {
        StartCoroutine(DisableAllSkillsCoroutine(duration));
    }

    private IEnumerator DisableAllSkillsCoroutine(float duration)
    {
        Button[] buttons = playerSkillPanel.GetComponentsInChildren<Button>();

        foreach (var btn in buttons)
            btn.interactable = false;

        yield return new WaitForSeconds(duration);

        if (!isPlayerDead && !isEnemyDead)
        {
            foreach (var btn in buttons)
                btn.interactable = true;
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

        if (!isPlayerDead && !isEnemyDead)
        {
            foreach (var btn in buttons)
                btn.interactable = true;
        }
    }

    private IEnumerator RunPassive(SkillData data)
    {
        while (true)
        {
            yield return new WaitForSeconds(data.cooldown);
            passiveSkill.Activate(data, player);
        }
    }

    public void OnPlayerDied()
    {
        isPlayerDead = true;
        DisableAllSkillButtonsPermanently();
    }

    public void OnEnemyDied()
    {
        isEnemyDead = true;
        DisableAllSkillButtonsPermanently();
    }

    private void DisableAllSkillButtonsPermanently()
    {
        Button[] buttons = playerSkillPanel.GetComponentsInChildren<Button>();
        foreach (var btn in buttons)
            btn.interactable = false;
    }
}
