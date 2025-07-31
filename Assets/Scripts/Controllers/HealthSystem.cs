using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour, IDamageable, IHealable
{
    public int maxHealth = 100;
    public int currentHealth;

    // Подія, що повідомляє про зміну HP
    public event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth); // виклик початкового значення
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
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
        // Тут можна викликати анімацію смерті або респаун
    }
}
