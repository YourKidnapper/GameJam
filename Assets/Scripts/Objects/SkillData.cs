using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    [TextArea] public string description;
    public float cooldown;
    public int power;
    public float multiplier = 1f;

    public int price;

    public Sprite icon;
    public int rarity;

    public SkillType type;
    public bool isPassive;
}
