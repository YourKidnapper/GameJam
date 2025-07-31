using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    [TextArea] public string description;
    public float cooldown;

    public int power;
    public Sprite icon;
    public int rarity; // 1 – common, 2 – rare, 3 – epic
}