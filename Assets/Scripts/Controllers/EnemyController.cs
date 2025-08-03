using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float attackCooldown = 2f;
    private float cooldownTimer;

    public SkillData[] attackSkills;
    public GameObject player;

    private HealthSystem healthSystem;
    private bool isInBerserkMode = false;
    private float nextAttackMultiplier = 1f;
    private bool isDead = false;

    private Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        cooldownTimer = attackCooldown;
        StartCoroutine(FindPlayerAndSubscribe());

        healthSystem.OnDeath += () =>
        {
            isDead = true;
            Debug.Log("❌ Ворог мертвий. Зупиняє дії.");

            // 🔥 Виклик екрану перемоги
            VictoryScreenManager victory = FindFirstObjectByType<VictoryScreenManager>();
            if (victory != null)
            {
                victory.PlayVictoryScreen();
            }
            else
            {
                Debug.LogWarning("⚠️ VictoryScreenManager не знайдено на сцені!");
            }
        };
    }

    private void Update()
    {
        if (isDead || player == null) return;

        // 🔥 Berserk Mode (25% HP)
        if (!isInBerserkMode && healthSystem.currentHealth <= healthSystem.maxHealth * 0.5f)
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
        Debug.Log("⚠️ Ворог активував BerserkMode!");
        PlayEnemyAnimation("BerserkMode");
    }

    private void TryAttack()
    {
        SkillData selected = ChooseSkill();
        if (selected == null) return;

        int damage = Mathf.RoundToInt(selected.power * selected.multiplier * nextAttackMultiplier);

        Debug.Log($"Ворог використовує {selected.skillName} і б'є на {damage} (x{nextAttackMultiplier})");

        // 🎬 Анімації для різних скілів
        switch (selected.skillName)
        {
            case "Attack":
                PlayEnemyAnimation("Attack");
                break;
            case "BerserkAttack":
                PlayEnemyAnimation("BerserkAttack");
                break;
            case "ComboAttack":
                PlayEnemyAnimation("ComboAttack");
                break;
            case "FireAttack":
                PlayEnemyAnimation("FireAttack");
                break;
            case "IceAttack":
                PlayEnemyAnimation("IceAttack");
                break;
            case "BerserkMode":
                PlayEnemyAnimation("BerserkMode");
                break;
        }

        // 🔊 Звук
        PlayEnemySound(selected.sfx);

        // Наносимо шкоду
        if (player.TryGetComponent(out IDamageable dmg))
        {
            dmg.TakeDamage(damage);
        }

        // Ефект Freeze
        if (selected.skillName == "IceAttack")
        {
            Debug.Log("❄️ Freeze активовано! Вимикаємо скіли гравця на 2 сек.");
            TryFreezePlayerSkills(2f);
        }

        nextAttackMultiplier = 1f;
    }

    private void PlayEnemyAnimation(string triggerName)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetTrigger(triggerName);
        }
        else
        {
            Debug.LogError("❌ У ворога немає Animator або він без контролера!");
        }
    }

    private void PlayEnemySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void TryFreezePlayerSkills(float duration)
    {
        SkillManager skillManager = FindFirstObjectByType<SkillManager>();
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
                if (skill.skillName == "BerserkAttack")
                    return skill;
            }
        }

        int index = Random.Range(0, attackSkills.Length);
        return attackSkills[index];
    }

    private IEnumerator FindPlayerAndSubscribe()
    {
        player = GameObject.FindWithTag("Player");
        while (player == null)
        {
            player = GameObject.FindWithTag("Player");
            yield return new WaitForSeconds(0.1f);
        }

        if (player.TryGetComponent(out HealthSystem playerHealth))
        {
            playerHealth.OnDeath += () =>
            {
                Debug.Log("🎯 Гравець помер. Ворог більше не атакує.");
                player = null;
            };
        }
    }
}