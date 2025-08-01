using UnityEngine;
using UnityEngine.UI;

public class SkillCardUI : MonoBehaviour
{
    public Image iconImage;
    public Text nameText;
    public Text priceText;
    public Button buyButton;

    private SkillData skillData;
    private SkillShopManager shopManager;

    public void Setup(SkillData data, SkillShopManager manager)
    {
        skillData = data;
        shopManager = manager;

        iconImage.sprite = data.icon;
        nameText.text = data.skillName;
        priceText.text = data.price.ToString();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => shopManager.BuySkill(skillData));
    }
}
