using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float attackCooldown = 2f;
    private float cooldownTimer;

    public SkillData[] attackSkills; // Сюди підкидаємо активні скіли ворога
    public GameObject player;        // Встановлюється вручну або шукається

    private HealthSystem healthSystem;
    private bool isInBerserkMode = false;
    private float nextAttackMultiplier = 1f;
    private bool isDead = false;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        cooldownTimer = attackCooldown;

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
            healthSystem.OnDeath += () =>
            {
            isDead = true;
            Debug.Log("❌ Ворог мертвий. Зупиняє дії.");
            };

        if (player != null && player.TryGetComponent(out HealthSystem playerHealth))
        {
            playerHealth.OnDeath += () =>
            {
                Debug.Log("🎯 Гравець помер. Ворог більше не атакує.");
                player = null;
            };
        }
    }

    private void Update()
    {
        if (player == null) return;

        if (isDead || player == null) return;

        // 🔥 Пасивний скіл — BerserkMod (25% HP)
        if (!isInBerserkMode && healthSystem.currentHealth <= healthSystem.maxHealth * 0.25f)
        {
            ActivateBerserkMode();
        }

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            TryAttack();
            cooldownTimer = attackCooldown;
        }
    }

    private void ActivateBerserkMode()
    {
        isInBerserkMode = true;
        nextAttackMultiplier = 3f;
        Debug.Log("⚠️ Ворог активував BerserkMod! Усі атаки тепер x3 урон!");
    }

    private void TryAttack()
    {
        SkillData selected = ChooseSkill();

        if (selected == null) return;

        // Розрахунок шкоди
        int damage = Mathf.RoundToInt(selected.power * selected.multiplier * nextAttackMultiplier);

        // Додаткова логіка для конкретних скілів
        if (selected.skillName == "Freeze")
        {
            Debug.Log("❄️ Freeze активовано! Гравець отримає 10 шкоди та всі скіли заморожені на 2 сек.");
            TryFreezePlayerSkills(2f); // 🔁 Ти маєш реалізувати цей метод у SkillManager
        }
        else if (selected.skillName == "PowerfulAttack")
        {
            Debug.Log("💥 PowerfulAttack активовано!");
        }

        Debug.Log($"Ворог використовує {selected.skillName} і б'є на {damage} (x{nextAttackMultiplier})");

        if (player.TryGetComponent(out IDamageable dmg))
        {
            dmg.TakeDamage(damage);
        }

        nextAttackMultiplier = 1f; // Скидаємо після кожної атаки
    }

    private void TryFreezePlayerSkills(float duration)
    {
        SkillManager skillManager = FindObjectOfType<SkillManager>();
        if (skillManager != null)
        {
            skillManager.DisableAllSkills(duration);
        }
    }

    private SkillData ChooseSkill()
    {
        if (healthSystem == null || attackSkills.Length == 0)
            return null;

        // Пріоритет Berserk атаки при <50%
        if (healthSystem.currentHealth < healthSystem.maxHealth / 2)
        {
            foreach (var skill in attackSkills)
            {
                if (skill.skillName == "Berserk")
                {
                    return skill;
                }
            }
        }

        // Інакше випадкова атака
        int index = Random.Range(0, attackSkills.Length);
        return attackSkills[index];
    }
}
