using UnityEngine;

public class AttackSkill : MonoBehaviour
{
    public void Activate(SkillData data, GameObject user, GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning($"❌ {data.skillName}: ціль атаки не задана.");
            return;
        }

        int damage = CalculateDamage(data, user);

        Debug.Log($"{user.name} використовує {data.skillName} проти {target.name}, наносячи {damage} шкоди!");

        if (target.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning($"❌ {target.name} не має компонента IDamageable.");
        }

        PlayAttackAnimation(user, data.skillName);
    }

    private int CalculateDamage(SkillData data, GameObject user)
    {
        float damage = data.power;

        if (user.TryGetComponent(out PlayerStats stats))
        {
            float boost = stats.ConsumeAttackMultiplier();
            damage *= boost;
        }

        damage *= data.multiplier;

        return Mathf.RoundToInt(damage);
    }

    private void PlayAttackAnimation(GameObject user, string skillName)
    {
        Animator anim = user.GetComponent<Animator>();
        if (anim != null && anim.runtimeAnimatorController != null)
        {
            if (HasTrigger(anim, skillName))
            {
                anim.SetTrigger(skillName);
            }
            else
            {
                Debug.LogWarning($"⚠️ У Animator відсутній тригер '{skillName}'.");
            }
        }
        else
        {
            Debug.LogError("❌ У користувача немає Animator або він без контролера!");
        }
    }

    private bool HasTrigger(Animator animator, string triggerName)
    {
        foreach (var param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger && param.name == triggerName)
                return true;
        }
        return false;
    }
}