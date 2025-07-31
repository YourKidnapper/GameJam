using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillButtonController : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;

    private SkillData skillData;
    private Button button;
    private bool isOnCooldown;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Setup(SkillData data)
    {
        skillData = data;
        iconImage.sprite = data.icon;

        button.onClick.RemoveAllListeners();

        if (skillData.isPassive)
        {
            button.interactable = false;
        }
        else
        {
            button.onClick.AddListener(TriggerSkill);
        }
    }

    private void TriggerSkill()
    {
        if (isOnCooldown) return;

        SkillManager.Instance.ActivateSkill(skillData);
        SkillManager.Instance.BlockSkillsDuringAnimation(0.5f);

        StartCooldown(skillData.cooldown);
    }

    private void StartCooldown(float cooldown)
    {
        isOnCooldown = true;

        iconImage.fillAmount = 0f;
        iconImage.type = Image.Type.Filled;
        iconImage.fillMethod = Image.FillMethod.Vertical;
        iconImage.fillOrigin = (int)Image.OriginVertical.Bottom;

        iconImage.fillAmount = 0f;
        iconImage.DOFillAmount(1f, cooldown)
            .SetEase(DG.Tweening.Ease.Linear)
            .OnComplete(() => { isOnCooldown = false; });
    }
}
