using UnityEngine;

public class SupportSkill : MonoBehaviour
{
    [SerializeField] private Animator Player_controller;
    public void Activate(SkillData data, GameObject user)
    {
        switch (data.skillName)
        {
            case "Heal":
                TriggerHeal(data, user);
                break;

            case "Power Up":
                TriggerPowerUp(data, user);
                break;

            case "Help of the GOD":
                TriggerHelpOfTheGods(data, user);
                break;

            default:
                Debug.LogWarning($"Support skill '{data.skillName}' не має реалізації!");
                break;
        }
    }

        private void TriggerHeal(SkillData data, GameObject user)
    {
        int heal = Mathf.RoundToInt(data.power * data.multiplier);
        Debug.Log($"{data.skillName} відновлює {heal} HP!");

        if (user.TryGetComponent(out IHealable healer))
        {
            healer.Heal(heal);

            // шукаємо Animator прямо на гравцеві
            Animator anim = user.GetComponent<Animator>();
            if (anim != null && anim.runtimeAnimatorController != null)
            {
                anim.SetTrigger("Heal");
            }
            else
            {
                Debug.LogError("❌ У гравця немає Animator або він без контролера!");
            }
        }
    }

    private void TriggerPowerUp(SkillData data, GameObject user)
    {
        float boost = data.multiplier;
        Debug.Log($"{data.skillName} підсилює наступну атаку на множник {boost}!");

        if (user.TryGetComponent(out PlayerStats stats))
        {
            stats.ApplyAttackBoost(boost);
        }
    }
    
    private void TriggerHelpOfTheGods(SkillData data, GameObject user)
    {
        float boost = data.multiplier;
        Debug.Log($"{data.skillName} активує допомогу богів!");

        if (user.TryGetComponent(out PlayerStats stats))
        {
            stats.ApplyAttackBoost(boost);
        }
    }
}
