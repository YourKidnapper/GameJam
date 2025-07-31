using UnityEngine;

public class AttackSkill : MonoBehaviour
{
    public void Activate(SkillData data, GameObject user, GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("Ціль атаки не задана.");
            return;
        }

        float damage = data.power;

        // Перевіряємо на буст
        if (user.TryGetComponent(out PlayerStats stats))
        {
            float boost = stats.ConsumeAttackMultiplier();
            damage *= boost;
        }

        int finalDamage = Mathf.RoundToInt(damage);
        Debug.Log($"{user.name} атакує {target.name} на {finalDamage} урону!");

        // Наносимо шкоду
        if (target.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(finalDamage);
        }
        else
        {
            Debug.LogWarning("Ціль не має компонента IDamageable.");
        }
    }
}
