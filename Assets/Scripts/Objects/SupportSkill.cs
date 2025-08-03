using UnityEngine;

public class SupportSkill : MonoBehaviour
{
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

        PlaySkillSound(user, data.sfx);
    }

    private void TriggerHeal(SkillData data, GameObject user)
    {
        int heal = Mathf.RoundToInt(data.power * data.multiplier);
        Debug.Log($"{data.skillName} відновлює {heal} HP!");

        if (user.TryGetComponent(out IHealable healer))
        {
            healer.Heal(heal);

            Animator anim = user.GetComponent<Animator>();
            if (anim != null && anim.runtimeAnimatorController != null)
            {
                anim.SetTrigger("Heal");
            }
        }
    }

    private void TriggerPowerUp(SkillData data, GameObject user)
    {
        float boost = data.multiplier;
        if (user.TryGetComponent(out PlayerStats stats))
            stats.ApplyAttackBoost(boost);
    }

    private void TriggerHelpOfTheGods(SkillData data, GameObject user)
    {
        float boost = data.multiplier;
        if (user.TryGetComponent(out PlayerStats stats))
            stats.ApplyAttackBoost(boost);
    }

    private void PlaySkillSound(GameObject user, AudioClip clip)
    {
        if (clip == null) return;

        AudioSource audio = user.GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("⚠️ У гравця немає AudioSource для відтворення звуку!");
        }
    }
}