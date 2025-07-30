using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public Image iconImage;
    private SkillTooltipTrigger tooltipTrigger;

    private void Awake()
    {
        tooltipTrigger = GetComponent<SkillTooltipTrigger>();
    }

    public void Setup(SkillData data)
    {
        // Налаштовуємо іконку
        iconImage.sprite = data.icon;

        // Налаштовуємо тултіп
        if (tooltipTrigger != null)
            tooltipTrigger.SetSkillData(data);
    }
}
