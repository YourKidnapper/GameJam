using UnityEngine;

public class PassiveSkill : MonoBehaviour
{
    public void Activate(SkillData data, GameObject user)
    {
        if (!data.isPassive)
        {
            Debug.LogWarning($"Скіл {data.skillName} не є пасивним, але викликаний як пасивка.");
            return;
        }

        switch (data.skillName)
        {
            case "Sleep":
                TriggerSleep(data, user);
                break;

            case "AutoHeal":
                TriggerAutoHeal(data, user);
                break;
                
            // case "Thorns":
            //     TriggerThorns(data, user);
            //     break;

            default:
                Debug.LogWarning($"Пасивка '{data.skillName}' не має реалізації.");
                break;
        }
    }

    private void TriggerSleep(SkillData data, GameObject user)
    {
        Debug.Log($"{data.skillName} активує ефект 'сон' — усі скіли в кулдаун на 5 секунд.");

        if (SkillManager.Instance != null)
        {
            SkillManager.Instance.DisableAllSkills(5f);
        }
    }

    private void TriggerAutoHeal(SkillData data, GameObject user)
    {
        int heal = Mathf.RoundToInt(data.power * data.multiplier);
        Debug.Log($"{data.skillName} автоматично хилить на {heal} HP!");

        if (user.TryGetComponent(out IHealable healer))
        {
            healer.Heal(heal);
        }
    }
}
