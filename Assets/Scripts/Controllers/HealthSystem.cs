using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour, IDamageable, IHealable
{
    public int maxHealth = 100;
    public int currentHealth;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        // 💰 Додаємо монети, якщо отримує урон ворог
        if (CompareTag("Enemy"))
        {
            PlayerData.Instance?.AddCoins(amount);
        }

        // 🔄 Оновлюємо UI HP
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
            OnDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} помер!");

        if (CompareTag("Player"))
        {
            SkillManager.Instance?.OnPlayerDied();
            UnityEngine.SceneManagement.SceneManager.LoadScene("ShopScene");
        }
        else if (CompareTag("Enemy"))
        {
            SkillManager.Instance?.OnEnemyDied();
        }
    }
}
